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

using Microsoft.Azure.Commands.Common.Authentication.Config.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Microsoft.Azure.Commands.Common.Authentication.Config.Internal
{
    /// <summary>
    /// Static helper class that allows binding strongly typed objects to configuration values.
    /// </summary>
    internal static class ConfigurationBinder
    {
        /// <summary>
        /// Attempts to bind the configuration instance to a new instance of type T.
        /// If this configuration section has a value, that will be used.
        /// Otherwise binding by matching property names against configuration keys recursively.
        /// </summary>
        /// <typeparam name="T">The type of the new instance to bind.</typeparam>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <returns>The new instance of T if successful, default(T) otherwise.</returns>
        public static (T, string) Get<T>(this IConfiguration configuration)
            => configuration.Get<T>(_ => { });

        /// <summary>
        /// Attempts to bind the configuration instance to a new instance of type T.
        /// If this configuration section has a value, that will be used.
        /// Otherwise binding by matching property names against configuration keys recursively.
        /// </summary>
        /// <typeparam name="T">The type of the new instance to bind.</typeparam>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <param name="configureOptions">Configures the binder options.</param>
        /// <returns>The new instance of T if successful, default(T) otherwise.</returns>
        public static (T, string) Get<T>(this IConfiguration configuration, Action<BinderOptions> configureOptions)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            (object result, string providerId) = configuration.Get(typeof(T), configureOptions);
            if (result == null)
            {
                return (default(T), providerId);
            }
            return ((T)result, providerId);
        }

        /// <summary>
        /// Attempts to bind the configuration instance to a new instance of type T.
        /// If this configuration section has a value, that will be used.
        /// Otherwise binding by matching property names against configuration keys recursively.
        /// </summary>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <param name="type">The type of the new instance to bind.</param>
        /// <returns>The new instance if successful, null otherwise.</returns>
        public static (object, string) Get(this IConfiguration configuration, Type type)
            => configuration.Get(type, _ => { });

        /// <summary>
        /// Attempts to bind the configuration instance to a new instance of type T.
        /// If this configuration section has a value, that will be used.
        /// Otherwise binding by matching property names against configuration keys recursively.
        /// </summary>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <param name="type">The type of the new instance to bind.</param>
        /// <param name="configureOptions">Configures the binder options.</param>
        /// <returns>The new instance if successful, null otherwise.</returns>
        public static (object, string) Get(this IConfiguration configuration, Type type, Action<BinderOptions> configureOptions)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = new BinderOptions();
            configureOptions?.Invoke(options);
            object bound = BindInstance(type, instance: null, config: configuration, options: options);
            string providerId = (configuration as IConfigurationSection).GetValueWithProviderId().Item2;
            return (bound, providerId);
        }

        /// <summary>
        /// Attempts to bind the given object instance to the configuration section specified by the key by matching property names against configuration keys recursively.
        /// </summary>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <param name="key">The key of the configuration section to bind.</param>
        /// <param name="instance">The object to bind.</param>
        public static void Bind(this IConfiguration configuration, string key, object instance)
            => configuration.GetSection(key).Bind(instance);

        /// <summary>
        /// Attempts to bind the given object instance to configuration values by matching property names against configuration keys recursively.
        /// </summary>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <param name="instance">The object to bind.</param>
        public static void Bind(this IConfiguration configuration, object instance)
            => configuration.Bind(instance, o => { });

        /// <summary>
        /// Attempts to bind the given object instance to configuration values by matching property names against configuration keys recursively.
        /// </summary>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <param name="instance">The object to bind.</param>
        /// <param name="configureOptions">Configures the binder options.</param>
        public static void Bind(this IConfiguration configuration, object instance, Action<BinderOptions> configureOptions)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (instance != null)
            {
                var options = new BinderOptions();
                configureOptions?.Invoke(options);
                BindInstance(instance.GetType(), instance, configuration, options);
            }
        }

        /// <summary>
        /// Extracts the value with the specified key and converts it to type T.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <returns>The converted value.</returns>
        public static T GetValue<T>(this IConfiguration configuration, string key)
        {
            return GetValue(configuration, key, default(T));
        }

        /// <summary>
        /// Extracts the value with the specified key and converts it to type T.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <param name="defaultValue">The default value to use if no value is found.</param>
        /// <returns>The converted value.</returns>
        public static T GetValue<T>(this IConfiguration configuration, string key, T defaultValue)
        {
            return (T)GetValue(configuration, typeof(T), key, defaultValue);
        }

        /// <summary>
        /// Extracts the value with the specified key and converts it to the specified type.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="type">The type to convert the value to.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <returns>The converted value.</returns>
        public static object GetValue(this IConfiguration configuration, Type type, string key)
        {
            return GetValue(configuration, type, key, defaultValue: null);
        }

        /// <summary>
        /// Extracts the value with the specified key and converts it to the specified type.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="type">The type to convert the value to.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <param name="defaultValue">The default value to use if no value is found.</param>
        /// <returns>The converted value.</returns>
        public static object GetValue(this IConfiguration configuration, Type type, string key, object defaultValue)
        {
            IConfigurationSection section = configuration.GetSection(key);
            string value = section.Value;
            if (value != null)
            {
                return ConvertValue(type, value, section.Path);
            }
            return defaultValue;
        }

        private static void BindNonScalar(this IConfiguration configuration, object instance, BinderOptions options)
        {
            if (instance != null)
            {
                foreach (PropertyInfo property in GetAllProperties(instance.GetType().GetTypeInfo()))
                {
                    BindProperty(property, instance, configuration, options);
                }
            }
        }

        private static void BindProperty(PropertyInfo property, object instance, IConfiguration config, BinderOptions options)
        {
            // We don't support set only, non public, or indexer properties
            if (property.GetMethod == null ||
                (!options.BindNonPublicProperties && !property.GetMethod.IsPublic) ||
                property.GetMethod.GetParameters().Length > 0)
            {
                return;
            }

            object propertyValue = property.GetValue(instance);
            bool hasSetter = property.SetMethod != null && (property.SetMethod.IsPublic || options.BindNonPublicProperties);

            if (propertyValue == null && !hasSetter)
            {
                // Property doesn't have a value and we cannot set it so there is no
                // point in going further down the graph
                return;
            }

            propertyValue = BindInstance(property.PropertyType, propertyValue, config.GetSection(property.Name), options);

            if (propertyValue != null && hasSetter)
            {
                property.SetValue(instance, propertyValue);
            }
        }

        private static object BindToCollection(TypeInfo typeInfo, IConfiguration config, BinderOptions options)
        {
            Type type = typeof(List<>).MakeGenericType(typeInfo.GenericTypeArguments[0]);
            object instance = Activator.CreateInstance(type);
            BindCollection(instance, type, config, options);
            return instance;
        }

        // Try to create an array/dictionary instance to back various collection interfaces
        private static object AttemptBindToCollectionInterfaces(Type type, IConfiguration config, BinderOptions options)
        {
            TypeInfo typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsInterface)
            {
                return null;
            }

            Type collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyList<>), type);
            if (collectionInterface != null)
            {
                // IEnumerable<T> is guaranteed to have exactly one parameter
                return BindToCollection(typeInfo, config, options);
            }

            collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyDictionary<,>), type);
            if (collectionInterface != null)
            {
                Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(typeInfo.GenericTypeArguments[0], typeInfo.GenericTypeArguments[1]);
                object instance = Activator.CreateInstance(dictionaryType);
                BindDictionary(instance, dictionaryType, config, options);
                return instance;
            }

            collectionInterface = FindOpenGenericInterface(typeof(IDictionary<,>), type);
            if (collectionInterface != null)
            {
                object instance = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(typeInfo.GenericTypeArguments[0], typeInfo.GenericTypeArguments[1]));
                BindDictionary(instance, collectionInterface, config, options);
                return instance;
            }

            collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyCollection<>), type);
            if (collectionInterface != null)
            {
                // IReadOnlyCollection<T> is guaranteed to have exactly one parameter
                return BindToCollection(typeInfo, config, options);
            }

            collectionInterface = FindOpenGenericInterface(typeof(ICollection<>), type);
            if (collectionInterface != null)
            {
                // ICollection<T> is guaranteed to have exactly one parameter
                return BindToCollection(typeInfo, config, options);
            }

            collectionInterface = FindOpenGenericInterface(typeof(IEnumerable<>), type);
            if (collectionInterface != null)
            {
                // IEnumerable<T> is guaranteed to have exactly one parameter
                return BindToCollection(typeInfo, config, options);
            }

            return null;
        }

        private static object BindInstance(Type type, object instance, IConfiguration config, BinderOptions options)
        {
            // if binding IConfigurationSection, break early
            if (type == typeof(IConfigurationSection))
            {
                return config;
            }

            var section = config as IConfigurationSection;
            string configValue = section?.Value;
            object convertedValue;
            Exception error;
            if (configValue != null && TryConvertValue(type, configValue, section.Path, out convertedValue, out error))
            {
                if (error != null)
                {
                    throw error;
                }

                // Leaf nodes are always reinitialized
                return convertedValue;
            }

            if (config != null && config.GetChildren().Any())
            {
                // If we don't have an instance, try to create one
                if (instance == null)
                {
                    // We are already done if binding to a new collection instance worked
                    instance = AttemptBindToCollectionInterfaces(type, config, options);
                    if (instance != null)
                    {
                        return instance;
                    }

                    instance = CreateInstance(type);
                }

                // See if its a Dictionary
                Type collectionInterface = FindOpenGenericInterface(typeof(IDictionary<,>), type);
                if (collectionInterface != null)
                {
                    BindDictionary(instance, collectionInterface, config, options);
                }
                else if (type.IsArray)
                {
                    instance = BindArray((Array)instance, config, options);
                }
                else
                {
                    // See if its an ICollection
                    collectionInterface = FindOpenGenericInterface(typeof(ICollection<>), type);
                    if (collectionInterface != null)
                    {
                        BindCollection(instance, collectionInterface, config, options);
                    }
                    // Something else
                    else
                    {
                        BindNonScalar(config, instance, options);
                    }
                }
            }

            return instance;
        }

        private static object CreateInstance(Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();

            if (typeInfo.IsInterface || typeInfo.IsAbstract)
            {
                throw new InvalidOperationException($"Error: cannot activate abstract class or interface, type: {type}");
            }

            if (type.IsArray)
            {
                if (typeInfo.GetArrayRank() > 1)
                {
                    throw new InvalidOperationException($"Error: multi-dimensional array is not supported, type: {type})");
                }

                return Array.CreateInstance(typeInfo.GetElementType(), 0);
            }

            if (!typeInfo.IsValueType)
            {
                bool hasDefaultConstructor = typeInfo.DeclaredConstructors.Any(ctor => ctor.IsPublic && ctor.GetParameters().Length == 0);
                if (!hasDefaultConstructor)
                {
                    throw new InvalidOperationException($"Error: missing parameterless constructor in type {type}");
                }
            }

            try
            {
                return Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error: failed to activate type [{type}]. {ex.Message}", ex);
            }
        }

        private static void BindDictionary(object dictionary, Type dictionaryType, IConfiguration config, BinderOptions options)
        {
            TypeInfo typeInfo = dictionaryType.GetTypeInfo();

            // IDictionary<K,V> is guaranteed to have exactly two parameters
            Type keyType = typeInfo.GenericTypeArguments[0];
            Type valueType = typeInfo.GenericTypeArguments[1];
            bool keyTypeIsEnum = keyType.GetTypeInfo().IsEnum;

            if (keyType != typeof(string) && !keyTypeIsEnum)
            {
                // We only support string and enum keys
                return;
            }

            PropertyInfo setter = typeInfo.GetDeclaredProperty("Item");
            foreach (IConfigurationSection child in config.GetChildren())
            {
                object item = BindInstance(
                    type: valueType,
                    instance: null,
                    config: child,
                    options: options);
                if (item != null)
                {
                    if (keyType == typeof(string))
                    {
                        string key = child.Key;
                        setter.SetValue(dictionary, item, new object[] { key });
                    }
                    else if (keyTypeIsEnum)
                    {
                        object key = Enum.Parse(keyType, child.Key);
                        setter.SetValue(dictionary, item, new object[] { key });
                    }
                }
            }
        }

        private static void BindCollection(object collection, Type collectionType, IConfiguration config, BinderOptions options)
        {
            TypeInfo typeInfo = collectionType.GetTypeInfo();

            // ICollection<T> is guaranteed to have exactly one parameter
            Type itemType = typeInfo.GenericTypeArguments[0];
            MethodInfo addMethod = typeInfo.GetDeclaredMethod("Add");

            foreach (IConfigurationSection section in config.GetChildren())
            {
                try
                {
                    object item = BindInstance(
                        type: itemType,
                        instance: null,
                        config: section,
                        options: options);
                    if (item != null)
                    {
                        addMethod.Invoke(collection, new[] { item });
                    }
                }
                catch
                {
                }
            }
        }

        private static Array BindArray(Array source, IConfiguration config, BinderOptions options)
        {
            IConfigurationSection[] children = config.GetChildren().ToArray();
            int arrayLength = source.Length;
            Type elementType = source.GetType().GetElementType();
            var newArray = Array.CreateInstance(elementType, arrayLength + children.Length);

            // binding to array has to preserve already initialized arrays with values
            if (arrayLength > 0)
            {
                Array.Copy(source, newArray, arrayLength);
            }

            for (int i = 0; i < children.Length; i++)
            {
                try
                {
                    object item = BindInstance(
                        type: elementType,
                        instance: null,
                        config: children[i],
                        options: options);
                    if (item != null)
                    {
                        newArray.SetValue(item, arrayLength + i);
                    }
                }
                catch
                {
                }
            }

            return newArray;
        }

        private static bool TryConvertValue(Type type, string value, string path, out object result, out Exception error)
        {
            error = null;
            result = null;
            if (type == typeof(object))
            {
                result = value;
                return true;
            }

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return true;
                }
                return TryConvertValue(Nullable.GetUnderlyingType(type), value, path, out result, out error);
            }

            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if (converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    result = converter.ConvertFromInvariantString(value);
                }
                catch (Exception ex)
                {
                    error = new InvalidOperationException($"Failed to convert value [{value}] to type [{type}].", ex);
                }
                return true;
            }

            return false;
        }

        private static object ConvertValue(Type type, string value, string path)
        {
            object result;
            Exception error;
            TryConvertValue(type, value, path, out result, out error);
            if (error != null)
            {
                throw error;
            }
            return result;
        }

        private static Type FindOpenGenericInterface(Type expected, Type actual)
        {
            TypeInfo actualTypeInfo = actual.GetTypeInfo();
            if (actualTypeInfo.IsGenericType &&
                actual.GetGenericTypeDefinition() == expected)
            {
                return actual;
            }

            IEnumerable<Type> interfaces = actualTypeInfo.ImplementedInterfaces;
            foreach (Type interfaceType in interfaces)
            {
                if (interfaceType.GetTypeInfo().IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == expected)
                {
                    return interfaceType;
                }
            }
            return null;
        }

        private static IEnumerable<PropertyInfo> GetAllProperties(TypeInfo type)
        {
            var allProperties = new List<PropertyInfo>();

            do
            {
                allProperties.AddRange(type.DeclaredProperties);
                type = type.BaseType.GetTypeInfo();
            }
            while (type != typeof(object).GetTypeInfo());

            return allProperties;
        }
    }
}
