// ----------------------------------------------------------------------
// <copyright file="GetMemberInfoAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetMemberInfoAction : BaseAction
    {
        public int IBCCustId { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "GetMemberInfo";
            }
        }
    }
}
