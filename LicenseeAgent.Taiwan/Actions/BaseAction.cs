// ----------------------------------------------------------------------
// <copyright file="BaseAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseAction
    {
        public string SiteName { get; set; }

        public abstract string ActionName { get; }
    }
}