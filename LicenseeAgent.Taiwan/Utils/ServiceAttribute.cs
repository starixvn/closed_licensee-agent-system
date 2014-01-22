// ----------------------------------------------------------------------
// <copyright file="ServiceAttribute.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ServiceAttribute : Attribute
    {
        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public bool IsWriteLog { get; set; }
    }
}
