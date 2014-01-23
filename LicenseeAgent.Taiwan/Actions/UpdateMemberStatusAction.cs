// ----------------------------------------------------------------------
// <copyright file="UpdateMemberStatusAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateMemberStatusAction : LogAction, IAction
    {
        public string SiteName { get; set; }

        public int? IBCCustId { get; set; }

        public int LASAgentId { get; set; }

        public bool IsClosed { get; set; }

        /// <summary>
        /// Get action name.
        /// </summary>
        /// <returns>Action name.</returns>
        public string GetActionName()
        {
            return "UpdateMemberStatus";
        }
    }
}
