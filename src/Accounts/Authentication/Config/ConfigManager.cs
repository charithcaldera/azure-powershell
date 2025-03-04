﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Commands.Common.Authentication.Abstractions;
using Microsoft.Azure.Commands.Common.Authentication.Config.Internal;
using Microsoft.Azure.Commands.Common.Authentication.Config.Internal.Interfaces;
using Microsoft.Azure.Commands.Common.Authentication.Config.Internal.Providers;
using Microsoft.Azure.Commands.Common.Exceptions;
using Microsoft.Azure.Commands.ResourceManager.Common;
using Microsoft.Azure.PowerShell.Common.Config;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Threading;

namespace Microsoft.Azure.Commands.Common.Authentication.Config
{
    /// <summary>
    /// Default implementation of <see cref="IConfigManager"/>, providing CRUD abilities to the configs.
    /// </summary>
    internal class ConfigManager : IConfigManager
    {
        /// <inheritdoc/>
        public string ConfigFilePath { get; private set; }

        private IConfigurationRoot _root;
        private readonly ConcurrentDictionary<string, ConfigDefinition> _configDefinitionMap = new ConcurrentDictionary<string, ConfigDefinition>(StringComparer.OrdinalIgnoreCase);
        private IOrderedEnumerable<KeyValuePair<string, ConfigDefinition>> OrderedConfigDefinitionMap => _configDefinitionMap.OrderBy(x => x.Key);
        private readonly ConcurrentDictionary<string, string> EnvironmentVariableToKeyMap = new ConcurrentDictionary<string, string>();
        private readonly IEnvironmentVariableProvider _environmentVariableProvider;
        private readonly IDataStore _dataStore;
        private readonly JsonConfigWriter _jsonConfigWriter;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        /// Creates an instance of <see cref="ConfigManager"/>.
        /// </summary>
        /// <param name="configFilePath">Path to the config file.</param>
        /// <param name="dataStore">Provider of file system APIs.</param>
        /// <param name="environmentVariableProvider">Provider of environment variable APIs.</param>
        internal ConfigManager(string configFilePath, IDataStore dataStore, IEnvironmentVariableProvider environmentVariableProvider)
        {
            _ = dataStore ?? throw new AzPSArgumentNullException($"{nameof(dataStore)} cannot be null.", nameof(dataStore));
            _ = configFilePath ?? throw new AzPSArgumentNullException($"{nameof(configFilePath)} cannot be null.", nameof(configFilePath));
            _ = environmentVariableProvider ?? throw new AzPSArgumentNullException($"{nameof(environmentVariableProvider)} cannot be null.", nameof(environmentVariableProvider));
            ConfigFilePath = configFilePath;
            _environmentVariableProvider = environmentVariableProvider;
            _dataStore = dataStore;
            _jsonConfigWriter = new JsonConfigWriter(ConfigFilePath, _dataStore);
        }

        /// <summary>
        /// Rebuild config hierarchy and load from the providers.
        /// </summary>
        public void BuildConfig()
        {
            var builder = new ConfigurationBuilder();

            if (SharedUtilities.IsWindowsPlatform())
            {
                // User and machine level environment variables are only on Windows
                builder.AddEnvironmentVariables(Constants.ConfigProviderIds.MachineEnvironment, new EnvironmentVariablesConfigurationOptions()
                {
                    EnvironmentVariableProvider = _environmentVariableProvider,
                    EnvironmentVariableTarget = EnvironmentVariableTarget.Machine,
                    EnvironmentVariableToKeyMap = EnvironmentVariableToKeyMap
                })
                    .AddEnvironmentVariables(Constants.ConfigProviderIds.UserEnvironment, new EnvironmentVariablesConfigurationOptions()
                    {
                        EnvironmentVariableProvider = _environmentVariableProvider,
                        EnvironmentVariableTarget = EnvironmentVariableTarget.User,
                        EnvironmentVariableToKeyMap = EnvironmentVariableToKeyMap
                    });
            }
            builder.AddJsonStream(Constants.ConfigProviderIds.UserConfig, _dataStore.ReadFileAsStream(ConfigFilePath))
                .AddEnvironmentVariables(Constants.ConfigProviderIds.ProcessEnvironment, new EnvironmentVariablesConfigurationOptions()
                {
                    EnvironmentVariableProvider = _environmentVariableProvider,
                    EnvironmentVariableTarget = EnvironmentVariableTarget.Process,
                    EnvironmentVariableToKeyMap = EnvironmentVariableToKeyMap
                })
                .AddUnsettableInMemoryCollection(Constants.ConfigProviderIds.ProcessConfig);

            _lock.EnterReadLock();
            try
            {
                _root = builder.Build();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        public void RegisterConfig(ConfigDefinition config)
        {
            // check if key already taken
            if (_configDefinitionMap.ContainsKey(config.Key))
            {
                if (_configDefinitionMap[config.Key] == config)
                {
                    Debug.WriteLine($"Config with key [{config.Key}] was registered twice");
                }
                else
                {
                    throw new AzPSArgumentException($"Duplicated config key. [{config.Key}] was already taken.", nameof(config.Key));
                }
                return;
            }
            // configure environment variable providers
            if (!string.IsNullOrEmpty(config.EnvironmentVariableName))
            {
                EnvironmentVariableToKeyMap[config.EnvironmentVariableName] = ConfigPathHelper.GetPathOfConfig(config.Key);
            }
            _configDefinitionMap[config.Key] = config;
        }

        /// <inheritdoc/>
        public T GetConfigValue<T>(string key, object invocation = null)
        {
            if (invocation != null && !(invocation is InvocationInfo))
            {
                throw new AzPSArgumentException($"Type error: type of {nameof(invocation)} must be {nameof(InvocationInfo)}", nameof(invocation));
            }
            return GetConfigValueInternal<T>(key, new InvocationInfoAdapter((InvocationInfo)invocation));
        }

        internal T GetConfigValueInternal<T>(string key, InternalInvocationInfo invocation) => (T)GetConfigValueInternal(key, invocation);

        internal object GetConfigValueInternal(string key, InternalInvocationInfo invocation)
        {
            _ = key ?? throw new AzPSArgumentNullException($"{nameof(key)} cannot be null.", nameof(key));
            if (!_configDefinitionMap.TryGetValue(key, out ConfigDefinition definition) || definition == null)
            {
                throw new AzPSArgumentException($"Config with key [{key}] was not registered.", nameof(key));
            }

            foreach (var path in ConfigPathHelper.EnumerateConfigPaths(key, invocation))
            {
                IConfigurationSection section = _root.GetSection(path);
                if (section.Exists())
                {
                    (object value, _) = GetConfigValueOrDefault(section, definition);
                    WriteDebug($"[ConfigManager] Got [{value}] from [{key}], Module = [{invocation?.ModuleName}], Cmdlet = [{invocation?.CmdletName}].");
                    return value;
                }
            }

            WriteDebug($"[ConfigManager] Got nothing from [{key}], Module = [{invocation?.ModuleName}], Cmdlet = [{invocation?.CmdletName}]. Returning default value [{definition.DefaultValue}].");
            return definition.DefaultValue;
        }

        private void WriteDebug(string message)
        {
            WriteMessage(message, AzureRMCmdlet.WriteDebugKey);
        }

        private void WriteMessage(string message, string eventHandlerKey)
        {
            try
            {
                if (AzureSession.Instance.TryGetComponent(eventHandlerKey, out EventHandler<StreamEventArgs> writeDebug))
                {
                    writeDebug.Invoke(this, new StreamEventArgs() { Message = message });
                }
            }
            catch (Exception)
            {
                // do not throw when session is not initialized
            }
        }

        private void WriteWarning(string message)
        {
            WriteMessage(message, AzureRMCmdlet.WriteWarningKey);
        }

        /// <inheritdoc/>
        public IEnumerable<ConfigDefinition> ListConfigDefinitions()
        {
            return OrderedConfigDefinitionMap.Select(x => x.Value);
        }

        /// <inheritdoc/>
        public IEnumerable<ConfigData> ListConfigs(ConfigFilter filter = null)
        {
            IList<ConfigData> results = new List<ConfigData>();

            // include all values
            ISet<string> noNeedForDefault = new HashSet<string>();
            foreach (var appliesToSection in _root.GetChildren())
            {
                foreach (var configSection in appliesToSection.GetChildren())
                {
                    string key = configSection.Key;
                    if (_configDefinitionMap.TryGetValue(key, out var configDefinition))
                    {
                        (object value, string providerId) = GetConfigValueOrDefault(configSection, configDefinition);
                        ConfigScope scope = ConfigScopeHelper.GetScopeByProviderId(providerId);
                        results.Add(new ConfigData(configDefinition, value, scope, appliesToSection.Key));
                        // if a config is already set at global level, there's no need to return its default value
                        if (string.Equals(ConfigFilter.GlobalAppliesTo, appliesToSection.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            noNeedForDefault.Add(configDefinition.Key);
                        }
                    }
                }
            }

            // include default values
            IEnumerable<string> keys = filter?.Keys ?? Enumerable.Empty<string>();
            bool isRegisteredKey(string key) => _configDefinitionMap.Keys.Contains(key, StringComparer.OrdinalIgnoreCase);
            IEnumerable<ConfigDefinition> configDefinitions = keys.Any() ? keys.Where(isRegisteredKey).Select(key => _configDefinitionMap[key]) : OrderedConfigDefinitionMap.Select(x => x.Value);
            configDefinitions.Where(x => !noNeedForDefault.Contains(x.Key)).Select(x => GetDefaultConfigData(x)).ForEach(x => results.Add(x));


            if (keys.Any())
            {
                results = results.Where(x => keys.Contains(x.Definition.Key, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            string appliesTo = filter?.AppliesTo;
            if (!string.IsNullOrEmpty(appliesTo))
            {
                results = results.Where(x => string.Equals(appliesTo, x.AppliesTo, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return results;
        }

        /// <summary>
        /// Get the value and the ID of the corresponding provider of the config.
        /// </summary>
        /// <param name="section">The section that stores the config.</param>
        /// <param name="definition">The definition of the config.</param>
        /// <returns>A tuple containing the value of the config and the ID of the provider from which the value is got.</returns>
        /// <remarks>Exceptions are handled gracefully in this method.</remarks>
        private (object value, string providerId) GetConfigValueOrDefault(IConfigurationSection section, ConfigDefinition definition)
        {
            try
            {
                return section.Get(definition.ValueType);
            }
            catch (InvalidOperationException ex)
            {
                WriteWarning($"[ConfigManager] Failed to get value for [{definition.Key}]. Using the default value [{definition.DefaultValue}] instead. Error: {ex.Message}. {ex.InnerException?.Message}");
                WriteDebug($"[ConfigManager] Exception: {ex.Message}, stack trace: \n{ex.StackTrace}");
                return (definition.DefaultValue, Constants.ConfigProviderIds.None);
            }
        }

        private ConfigData GetDefaultConfigData(ConfigDefinition configDefinition)
        {
            return new ConfigData(configDefinition,
                configDefinition.DefaultValue,
                ConfigScope.Default,
                ConfigFilter.GlobalAppliesTo);
        }

        // A bulk update API is currently unnecessary as we don't expect users to do that.
        // But if telemetry data proves it's a demanded feature, we might add it in the future.
        // public IEnumerable<Config> UpdateConfigs(IEnumerable<UpdateConfigOptions> updateConfigOptions) => updateConfigOptions.Select(UpdateConfig);

        /// <inheritdoc/>
        public ConfigData UpdateConfig(string key, object value, ConfigScope scope)
        {
            return UpdateConfig(new UpdateConfigOptions(key, value, scope));
        }

        /// <inheritdoc/>
        public ConfigData UpdateConfig(UpdateConfigOptions options)
        {
            if (options == null)
            {
                throw new AzPSArgumentNullException($"{nameof(options)} cannot be null when updating config.", nameof(options));
            }

            if (!_configDefinitionMap.TryGetValue(options.Key, out ConfigDefinition definition) || definition == null)
            {
                throw new AzPSArgumentException($"Config with key [{options.Key}] was not registered.", nameof(options.Key));
            }

            try
            {
                definition.Validate(options.Value);
            }
            catch (Exception e)
            {
                throw new AzPSArgumentException(e.Message, e);
            }

            if (AppliesToHelper.TryParseAppliesTo(options.AppliesTo, out var appliesTo) && !definition.CanApplyTo.Contains(appliesTo))
            {
                throw new AzPSArgumentException($"[{options.AppliesTo}] is not a valid value for AppliesTo - it doesn't match any of ({AppliesToHelper.FormatOptions(definition.CanApplyTo)}).", nameof(options.AppliesTo));
            }

            definition.Apply(options.Value);

            string path = ConfigPathHelper.GetPathOfConfig(options.Key, options.AppliesTo);

            switch (options.Scope)
            {
                case ConfigScope.Process:
                    SetProcessLevelConfig(path, options.Value);
                    break;
                case ConfigScope.CurrentUser:
                    SetUserLevelConfig(path, options.Value);
                    break;
            }

            WriteDebug($"[ConfigManager] Updated [{options.Key}] to [{options.Value}]. Scope = [{options.Scope}], AppliesTo = [{options.AppliesTo}]");

            return new ConfigData(definition, options.Value, options.Scope, options.AppliesTo);
        }

        private void SetProcessLevelConfig(string path, object value)
        {
            GetProcessLevelConfigProvider().Set(path, value.ToString());
        }

        private UnsettableMemoryConfigurationProvider GetProcessLevelConfigProvider()
        {
            return _root.GetConfigurationProvider(Constants.ConfigProviderIds.ProcessConfig) as UnsettableMemoryConfigurationProvider;
        }

        private void SetUserLevelConfig(string path, object value)
        {
            _lock.EnterWriteLock();
            try
            {
                _jsonConfigWriter.Update(path, value);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            BuildConfig(); // reload the config values
        }

        /// <inheritdoc/>
        public void ClearConfig(string key, ConfigScope scope) => ClearConfig(new ClearConfigOptions(key, scope));

        /// <inheritdoc/>
        public void ClearConfig(ClearConfigOptions options)
        {
            _ = options ?? throw new AzPSArgumentNullException($"{nameof(options)} cannot be null.", nameof(options));

            bool clearAll = string.IsNullOrEmpty(options.Key);

            if (clearAll)
            {
                ClearAllConfigs(options);
            }
            else
            {
                ClearConfigByKey(options);
            }
        }

        private void ClearAllConfigs(ClearConfigOptions options)
        {
            switch (options.Scope)
            {
                case ConfigScope.Process:
                    ClearProcessLevelAllConfigs(options);
                    break;
                case ConfigScope.CurrentUser:
                    ClearUserLevelAllConfigs(options);
                    break;
                default:
                    throw new AzPSArgumentException($"[{options.Scope}] is not a valid scope when clearing configs.", nameof(options.Scope));
            }
            WriteDebug($"[ConfigManager] Cleared all the configs. Scope = [{options.Scope}].");
        }

        private void ClearProcessLevelAllConfigs(ClearConfigOptions options)
        {
            var configProvider = GetProcessLevelConfigProvider();
            if (string.IsNullOrEmpty(options.AppliesTo))
            {
                configProvider.UnsetAll();
            }
            else
            {
                foreach (var key in _configDefinitionMap.Keys)
                {
                    configProvider.Unset(ConfigPathHelper.GetPathOfConfig(key, options.AppliesTo));
                }
            }
        }

        private void ClearUserLevelAllConfigs(ClearConfigOptions options)
        {
            _lock.EnterWriteLock();
            try
            {
                if (string.IsNullOrEmpty(options.AppliesTo))
                {
                    _jsonConfigWriter.ClearAll();
                }
                else
                {
                    foreach (var key in _configDefinitionMap.Keys)
                    {
                        _jsonConfigWriter.Clear(ConfigPathHelper.GetPathOfConfig(key, options.AppliesTo));
                    }
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            BuildConfig();
        }

        private void ClearConfigByKey(ClearConfigOptions options)
        {
            if (!_configDefinitionMap.TryGetValue(options.Key, out ConfigDefinition definition))
            {
                throw new AzPSArgumentException($"Config with key [{options.Key}] was not registered.", nameof(options.Key));
            }

            string path = ConfigPathHelper.GetPathOfConfig(definition.Key, options.AppliesTo);

            switch (options.Scope)
            {
                case ConfigScope.Process:
                    GetProcessLevelConfigProvider().Unset(path);
                    break;
                case ConfigScope.CurrentUser:
                    ClearUserLevelConfigByKey(path);
                    break;
            }

            WriteDebug($"[ConfigManager] Cleared [{options.Key}]. Scope = [{options.Scope}], AppliesTo = [{options.AppliesTo}]");
        }

        private void ClearUserLevelConfigByKey(string key)
        {
            _lock.EnterWriteLock();
            try
            {
                _jsonConfigWriter.Clear(key);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            BuildConfig();
        }
    }
}
