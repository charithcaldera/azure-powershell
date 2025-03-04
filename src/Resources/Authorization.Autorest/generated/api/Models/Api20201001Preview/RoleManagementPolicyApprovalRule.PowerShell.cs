// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

namespace Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview
{
    using Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Runtime.PowerShell;

    /// <summary>The role management policy rule.</summary>
    [System.ComponentModel.TypeConverter(typeof(RoleManagementPolicyApprovalRuleTypeConverter))]
    public partial class RoleManagementPolicyApprovalRule
    {

        /// <summary>
        /// <c>AfterDeserializeDictionary</c> will be called after the deserialization has finished, allowing customization of the
        /// object before it is returned. Implement this method in a partial class to enable this behavior
        /// </summary>
        /// <param name="content">The global::System.Collections.IDictionary content that should be used.</param>

        partial void AfterDeserializeDictionary(global::System.Collections.IDictionary content);

        /// <summary>
        /// <c>AfterDeserializePSObject</c> will be called after the deserialization has finished, allowing customization of the object
        /// before it is returned. Implement this method in a partial class to enable this behavior
        /// </summary>
        /// <param name="content">The global::System.Management.Automation.PSObject content that should be used.</param>

        partial void AfterDeserializePSObject(global::System.Management.Automation.PSObject content);

        /// <summary>
        /// <c>BeforeDeserializeDictionary</c> will be called before the deserialization has commenced, allowing complete customization
        /// of the object before it is deserialized.
        /// If you wish to disable the default deserialization entirely, return <c>true</c> in the <see "returnNow" /> output parameter.
        /// Implement this method in a partial class to enable this behavior.
        /// </summary>
        /// <param name="content">The global::System.Collections.IDictionary content that should be used.</param>
        /// <param name="returnNow">Determines if the rest of the serialization should be processed, or if the method should return
        /// instantly.</param>

        partial void BeforeDeserializeDictionary(global::System.Collections.IDictionary content, ref bool returnNow);

        /// <summary>
        /// <c>BeforeDeserializePSObject</c> will be called before the deserialization has commenced, allowing complete customization
        /// of the object before it is deserialized.
        /// If you wish to disable the default deserialization entirely, return <c>true</c> in the <see "returnNow" /> output parameter.
        /// Implement this method in a partial class to enable this behavior.
        /// </summary>
        /// <param name="content">The global::System.Management.Automation.PSObject content that should be used.</param>
        /// <param name="returnNow">Determines if the rest of the serialization should be processed, or if the method should return
        /// instantly.</param>

        partial void BeforeDeserializePSObject(global::System.Management.Automation.PSObject content, ref bool returnNow);

        /// <summary>
        /// Deserializes a <see cref="global::System.Collections.IDictionary" /> into an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.RoleManagementPolicyApprovalRule"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Collections.IDictionary content that should be used.</param>
        /// <returns>
        /// an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRule"
        /// />.
        /// </returns>
        public static Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRule DeserializeFromDictionary(global::System.Collections.IDictionary content)
        {
            return new RoleManagementPolicyApprovalRule(content);
        }

        /// <summary>
        /// Deserializes a <see cref="global::System.Management.Automation.PSObject" /> into an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.RoleManagementPolicyApprovalRule"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Management.Automation.PSObject content that should be used.</param>
        /// <returns>
        /// an instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRule"
        /// />.
        /// </returns>
        public static Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRule DeserializeFromPSObject(global::System.Management.Automation.PSObject content)
        {
            return new RoleManagementPolicyApprovalRule(content);
        }

        /// <summary>
        /// Creates a new instance of <see cref="RoleManagementPolicyApprovalRule" />, deserializing the content from a json string.
        /// </summary>
        /// <param name="jsonText">a string containing a JSON serialized instance of this model.</param>
        /// <returns>an instance of the <see cref="className" /> model class.</returns>
        public static Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRule FromJsonString(string jsonText) => FromJson(Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Runtime.Json.JsonNode.Parse(jsonText));

        /// <summary>
        /// Deserializes a <see cref="global::System.Collections.IDictionary" /> into a new instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.RoleManagementPolicyApprovalRule"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Collections.IDictionary content that should be used.</param>
        internal RoleManagementPolicyApprovalRule(global::System.Collections.IDictionary content)
        {
            bool returnNow = false;
            BeforeDeserializeDictionary(content, ref returnNow);
            if (returnNow)
            {
                return;
            }
            // actually deserialize
            if (content.Contains("Setting"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).Setting = (Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IApprovalSettings) content.GetValueForProperty("Setting",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).Setting, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.ApprovalSettingsTypeConverter.ConvertFrom);
            }
            if (content.Contains("TargetCaller"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetCaller = (string) content.GetValueForProperty("TargetCaller",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetCaller, global::System.Convert.ToString);
            }
            if (content.Contains("TargetOperation"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetOperation = (string[]) content.GetValueForProperty("TargetOperation",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetOperation, __y => TypeConverterExtensions.SelectToArray<string>(__y, global::System.Convert.ToString));
            }
            if (content.Contains("TargetLevel"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetLevel = (string) content.GetValueForProperty("TargetLevel",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetLevel, global::System.Convert.ToString);
            }
            if (content.Contains("TargetObject"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetObject = (string[]) content.GetValueForProperty("TargetObject",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetObject, __y => TypeConverterExtensions.SelectToArray<string>(__y, global::System.Convert.ToString));
            }
            if (content.Contains("TargetInheritableSetting"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetInheritableSetting = (string[]) content.GetValueForProperty("TargetInheritableSetting",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetInheritableSetting, __y => TypeConverterExtensions.SelectToArray<string>(__y, global::System.Convert.ToString));
            }
            if (content.Contains("TargetEnforcedSetting"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetEnforcedSetting = (string[]) content.GetValueForProperty("TargetEnforcedSetting",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetEnforcedSetting, __y => TypeConverterExtensions.SelectToArray<string>(__y, global::System.Convert.ToString));
            }
            if (content.Contains("Target"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).Target = (Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleTarget) content.GetValueForProperty("Target",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).Target, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.RoleManagementPolicyRuleTargetTypeConverter.ConvertFrom);
            }
            if (content.Contains("Id"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).Id = (string) content.GetValueForProperty("Id",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).Id, global::System.Convert.ToString);
            }
            if (content.Contains("RuleType"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).RuleType = (Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Support.RoleManagementPolicyRuleType) content.GetValueForProperty("RuleType",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).RuleType, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Support.RoleManagementPolicyRuleType.CreateFrom);
            }
            if (content.Contains("SettingApprovalMode"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingApprovalMode = (Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Support.ApprovalMode?) content.GetValueForProperty("SettingApprovalMode",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingApprovalMode, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Support.ApprovalMode.CreateFrom);
            }
            if (content.Contains("SettingIsApprovalRequired"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsApprovalRequired = (bool?) content.GetValueForProperty("SettingIsApprovalRequired",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsApprovalRequired, (__y)=> (bool) global::System.Convert.ChangeType(__y, typeof(bool)));
            }
            if (content.Contains("SettingIsApprovalRequiredForExtension"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsApprovalRequiredForExtension = (bool?) content.GetValueForProperty("SettingIsApprovalRequiredForExtension",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsApprovalRequiredForExtension, (__y)=> (bool) global::System.Convert.ChangeType(__y, typeof(bool)));
            }
            if (content.Contains("SettingIsRequestorJustificationRequired"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsRequestorJustificationRequired = (bool?) content.GetValueForProperty("SettingIsRequestorJustificationRequired",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsRequestorJustificationRequired, (__y)=> (bool) global::System.Convert.ChangeType(__y, typeof(bool)));
            }
            if (content.Contains("SettingApprovalStage"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingApprovalStage = (Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IApprovalStage[]) content.GetValueForProperty("SettingApprovalStage",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingApprovalStage, __y => TypeConverterExtensions.SelectToArray<Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IApprovalStage>(__y, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.ApprovalStageTypeConverter.ConvertFrom));
            }
            AfterDeserializeDictionary(content);
        }

        /// <summary>
        /// Deserializes a <see cref="global::System.Management.Automation.PSObject" /> into a new instance of <see cref="Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.RoleManagementPolicyApprovalRule"
        /// />.
        /// </summary>
        /// <param name="content">The global::System.Management.Automation.PSObject content that should be used.</param>
        internal RoleManagementPolicyApprovalRule(global::System.Management.Automation.PSObject content)
        {
            bool returnNow = false;
            BeforeDeserializePSObject(content, ref returnNow);
            if (returnNow)
            {
                return;
            }
            // actually deserialize
            if (content.Contains("Setting"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).Setting = (Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IApprovalSettings) content.GetValueForProperty("Setting",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).Setting, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.ApprovalSettingsTypeConverter.ConvertFrom);
            }
            if (content.Contains("TargetCaller"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetCaller = (string) content.GetValueForProperty("TargetCaller",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetCaller, global::System.Convert.ToString);
            }
            if (content.Contains("TargetOperation"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetOperation = (string[]) content.GetValueForProperty("TargetOperation",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetOperation, __y => TypeConverterExtensions.SelectToArray<string>(__y, global::System.Convert.ToString));
            }
            if (content.Contains("TargetLevel"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetLevel = (string) content.GetValueForProperty("TargetLevel",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetLevel, global::System.Convert.ToString);
            }
            if (content.Contains("TargetObject"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetObject = (string[]) content.GetValueForProperty("TargetObject",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetObject, __y => TypeConverterExtensions.SelectToArray<string>(__y, global::System.Convert.ToString));
            }
            if (content.Contains("TargetInheritableSetting"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetInheritableSetting = (string[]) content.GetValueForProperty("TargetInheritableSetting",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetInheritableSetting, __y => TypeConverterExtensions.SelectToArray<string>(__y, global::System.Convert.ToString));
            }
            if (content.Contains("TargetEnforcedSetting"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetEnforcedSetting = (string[]) content.GetValueForProperty("TargetEnforcedSetting",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).TargetEnforcedSetting, __y => TypeConverterExtensions.SelectToArray<string>(__y, global::System.Convert.ToString));
            }
            if (content.Contains("Target"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).Target = (Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleTarget) content.GetValueForProperty("Target",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).Target, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.RoleManagementPolicyRuleTargetTypeConverter.ConvertFrom);
            }
            if (content.Contains("Id"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).Id = (string) content.GetValueForProperty("Id",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).Id, global::System.Convert.ToString);
            }
            if (content.Contains("RuleType"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).RuleType = (Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Support.RoleManagementPolicyRuleType) content.GetValueForProperty("RuleType",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyRuleInternal)this).RuleType, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Support.RoleManagementPolicyRuleType.CreateFrom);
            }
            if (content.Contains("SettingApprovalMode"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingApprovalMode = (Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Support.ApprovalMode?) content.GetValueForProperty("SettingApprovalMode",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingApprovalMode, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Support.ApprovalMode.CreateFrom);
            }
            if (content.Contains("SettingIsApprovalRequired"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsApprovalRequired = (bool?) content.GetValueForProperty("SettingIsApprovalRequired",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsApprovalRequired, (__y)=> (bool) global::System.Convert.ChangeType(__y, typeof(bool)));
            }
            if (content.Contains("SettingIsApprovalRequiredForExtension"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsApprovalRequiredForExtension = (bool?) content.GetValueForProperty("SettingIsApprovalRequiredForExtension",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsApprovalRequiredForExtension, (__y)=> (bool) global::System.Convert.ChangeType(__y, typeof(bool)));
            }
            if (content.Contains("SettingIsRequestorJustificationRequired"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsRequestorJustificationRequired = (bool?) content.GetValueForProperty("SettingIsRequestorJustificationRequired",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingIsRequestorJustificationRequired, (__y)=> (bool) global::System.Convert.ChangeType(__y, typeof(bool)));
            }
            if (content.Contains("SettingApprovalStage"))
            {
                ((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingApprovalStage = (Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IApprovalStage[]) content.GetValueForProperty("SettingApprovalStage",((Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IRoleManagementPolicyApprovalRuleInternal)this).SettingApprovalStage, __y => TypeConverterExtensions.SelectToArray<Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.IApprovalStage>(__y, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Models.Api20201001Preview.ApprovalStageTypeConverter.ConvertFrom));
            }
            AfterDeserializePSObject(content);
        }

        /// <summary>Serializes this instance to a json string.</summary>

        /// <returns>a <see cref="System.String" /> containing this model serialized to JSON text.</returns>
        public string ToJsonString() => ToJson(null, Microsoft.Azure.PowerShell.Cmdlets.Resources.Authorization.Runtime.SerializationMode.IncludeAll)?.ToString();
    }
    /// The role management policy rule.
    [System.ComponentModel.TypeConverter(typeof(RoleManagementPolicyApprovalRuleTypeConverter))]
    public partial interface IRoleManagementPolicyApprovalRule

    {

    }
}