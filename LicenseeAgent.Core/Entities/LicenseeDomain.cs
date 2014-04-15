// --------------------------------------------------------------------------
// <copyright file="LicenseeDomain.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Entities
{
    /// <summary>
    /// LicenseeDomain class.
    /// </summary>
    public class LicenseeDomain
    {
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// Licensee id.
        /// </value>
        public int LicenseeId { get; set; }

        public string Environment { get; set; }
    }
}