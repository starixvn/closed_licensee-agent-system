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
    public class GetMemberInfoListAction : BaseAction
    {
        public int LASAgentId { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "GetMemberInfoList";
            }
        }
    }
}