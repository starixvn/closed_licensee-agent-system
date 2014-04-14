// ----------------------------------------------------------------------
// <copyright file="GetMemberInfoListAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Taiwan.Attributes;

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