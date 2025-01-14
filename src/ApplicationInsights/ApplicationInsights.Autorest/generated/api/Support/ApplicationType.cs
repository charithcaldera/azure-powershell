// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

namespace Microsoft.Azure.PowerShell.Cmdlets.ApplicationInsights.Support
{

    /// <summary>Type of application being monitored.</summary>
    public partial struct ApplicationType :
        System.IEquatable<ApplicationType>
    {
        public static Microsoft.Azure.PowerShell.Cmdlets.ApplicationInsights.Support.ApplicationType Other = @"other";

        public static Microsoft.Azure.PowerShell.Cmdlets.ApplicationInsights.Support.ApplicationType Web = @"web";

        /// <summary>the value for an instance of the <see cref="ApplicationType" /> Enum.</summary>
        private string _value { get; set; }

        /// <summary>Creates an instance of the <see cref="ApplicationType" Enum class./></summary>
        /// <param name="underlyingValue">the value to create an instance for.</param>
        private ApplicationType(string underlyingValue)
        {
            this._value = underlyingValue;
        }

        /// <summary>Conversion from arbitrary object to ApplicationType</summary>
        /// <param name="value">the value to convert to an instance of <see cref="ApplicationType" />.</param>
        internal static object CreateFrom(object value)
        {
            return new ApplicationType(global::System.Convert.ToString(value));
        }

        /// <summary>Compares values of enum type ApplicationType</summary>
        /// <param name="e">the value to compare against this instance.</param>
        /// <returns><c>true</c> if the two instances are equal to the same value</returns>
        public bool Equals(Microsoft.Azure.PowerShell.Cmdlets.ApplicationInsights.Support.ApplicationType e)
        {
            return _value.Equals(e._value);
        }

        /// <summary>Compares values of enum type ApplicationType (override for Object)</summary>
        /// <param name="obj">the value to compare against this instance.</param>
        /// <returns><c>true</c> if the two instances are equal to the same value</returns>
        public override bool Equals(object obj)
        {
            return obj is ApplicationType && Equals((ApplicationType)obj);
        }

        /// <summary>Returns hashCode for enum ApplicationType</summary>
        /// <returns>The hashCode of the value</returns>
        public override int GetHashCode()
        {
            return this._value.GetHashCode();
        }

        /// <summary>Returns string representation for ApplicationType</summary>
        /// <returns>A string for this value.</returns>
        public override string ToString()
        {
            return this._value;
        }

        /// <summary>Implicit operator to convert string to ApplicationType</summary>
        /// <param name="value">the value to convert to an instance of <see cref="ApplicationType" />.</param>

        public static implicit operator ApplicationType(string value)
        {
            return new ApplicationType(value);
        }

        /// <summary>Implicit operator to convert ApplicationType to string</summary>
        /// <param name="e">the value to convert to an instance of <see cref="ApplicationType" />.</param>

        public static implicit operator string(Microsoft.Azure.PowerShell.Cmdlets.ApplicationInsights.Support.ApplicationType e)
        {
            return e._value;
        }

        /// <summary>Overriding != operator for enum ApplicationType</summary>
        /// <param name="e1">the value to compare against <see cref="e2" /></param>
        /// <param name="e2">the value to compare against <see cref="e1" /></param>
        /// <returns><c>true</c> if the two instances are not equal to the same value</returns>
        public static bool operator !=(Microsoft.Azure.PowerShell.Cmdlets.ApplicationInsights.Support.ApplicationType e1, Microsoft.Azure.PowerShell.Cmdlets.ApplicationInsights.Support.ApplicationType e2)
        {
            return !e2.Equals(e1);
        }

        /// <summary>Overriding == operator for enum ApplicationType</summary>
        /// <param name="e1">the value to compare against <see cref="e2" /></param>
        /// <param name="e2">the value to compare against <see cref="e1" /></param>
        /// <returns><c>true</c> if the two instances are equal to the same value</returns>
        public static bool operator ==(Microsoft.Azure.PowerShell.Cmdlets.ApplicationInsights.Support.ApplicationType e1, Microsoft.Azure.PowerShell.Cmdlets.ApplicationInsights.Support.ApplicationType e2)
        {
            return e2.Equals(e1);
        }
    }
}