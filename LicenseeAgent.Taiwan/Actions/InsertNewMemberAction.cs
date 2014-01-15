// ----------------------------------------------------------------------
// <copyright file="InsertNewMemberAction.cs" company="Nexcel Solutions Vietnam">
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
    public class InsertNewMemberAction : IAction
    {
        [Service(Name = "SiteName")]
        public string SiteName { get; set; }

        [Service(Name = "username")]
        public string UserName { get; set; }

        [Service(Name = "password")]
        public string Password { get; set; }

        [Service(Name = "IBCCustId")]
        public int IBCCustId { get; set; }

        [Service(Name = "IBCUserName")]
        public string IBCUserName { get; set; }

        [Service(Name = "firstname")]
        public string FirstName { get; set; }

        [Service(Name = "lastname")]
        public string LastName { get; set; }

        [Service(Name = "phone")]
        public string Phone { get; set; }

        [Service(Name = "otherphone")]
        public string OtherPhone { get; set; }

        [Service(Name = "fax")]
        public string Fax { get; set; }

        [Service(Name = "currency")]
        public int Currency { get; set; }

        [Service(Name = "LASAgentID")]
        public int LASAgentId { get; set; }

        /// <summary>
        /// Get action name.
        /// </summary>
        /// <returns>Action name.</returns>
        public string GetActionName()
        {
            return "InsertNewMember";
        }
    }
}
