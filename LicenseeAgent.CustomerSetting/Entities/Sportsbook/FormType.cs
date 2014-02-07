// --------------------------------------------------------------------------
// <copyright file="FormType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.30">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Entities.Sportsbook
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum FormType : int
    {
        /// <summary>
        /// Add new form (agent or member).
        /// </summary>
        Add = 0,

        /// <summary>
        /// Edit directly form (agent or member).
        /// </summary>
        Edit = 1,

        /// <summary>
        /// ReadOnly form.
        /// </summary>
        ReadOnly = 2,

        /// <summary>
        /// Edit indirect member.
        /// </summary>
        EditIndirectMember = 3,

        /// <summary>
        /// Edit multiple member.
        /// </summary>
        EditMultiple = 4
    }
}