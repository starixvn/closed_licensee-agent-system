// ----------------------------------------------------------------------
// <copyright file="GetMemberInfoAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetMemberInfoAction : IAction
    {
        public string SiteName { get; set; }

        public int IBCCustId { get; set; }

        [Service]
        public string ActionName
        {
            get
            {
                return "GetMemberInfo";
            }
        }
    }
}
