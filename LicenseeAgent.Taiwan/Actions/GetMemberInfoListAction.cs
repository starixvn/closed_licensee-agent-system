// ----------------------------------------------------------------------
// <copyright file="GetMemberInfoListAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetMemberInfoListAction : IAction
    {
        public string SiteName { get; set; }

        public int LASAgentId { get; set; }

        [Service]
        public string ActionName
        {
            get
            {
                return "GetMemberInfoList";
            }
        }
    }
}
