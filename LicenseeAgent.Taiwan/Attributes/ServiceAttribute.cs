// ----------------------------------------------------------------------
// <copyright file="ServiceAttribute.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Attributes
{
    using System;

    /// <summary>
    /// ServiceAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ServiceAttribute : Attribute
    {
        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public bool IsParamAction { get; set; }
    }
}
