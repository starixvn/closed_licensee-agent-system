// --------------------------------------------------------------------------
// <copyright file="LicenseeDomain.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseeDomain
    {
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// Licensee name.
        /// </value>
        public string Licensee { get; set; }
    }
}