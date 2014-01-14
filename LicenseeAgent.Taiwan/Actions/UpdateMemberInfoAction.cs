// ----------------------------------------------------------------------
// <copyright file="UpdateMemberInfoAction.cs" company="Nexcel Solutions Vietnam">
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
    public class UpdateMemberInfoAction : IAction
    {
        public string SiteName { get; set; }

        public int IBCCustId { get; set; }

        public string Phone { get; set; }

        public string OtherPhone { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Fax { get; set; }

        /// <summary>
        /// Get action name.
        /// </summary>
        /// <returns>Action name.</returns>
        public string GetActionName()
        {
            return "UpdateMemberInfo";
        }
    }
}
