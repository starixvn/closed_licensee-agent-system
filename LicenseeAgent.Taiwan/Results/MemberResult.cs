// ----------------------------------------------------------------------
// <copyright file="MemberResult.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Results
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class MemberResult
    {
        public int IBCCustId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public bool IsClosed { get; set; }
    }
}
