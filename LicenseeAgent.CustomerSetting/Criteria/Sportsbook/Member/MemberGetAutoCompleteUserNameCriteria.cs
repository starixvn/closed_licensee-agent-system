// ----------------------------------------------------------------------
// <copyright file="MemberGetAutoCompleteUserNameCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Member
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberGetAutoCompleteUserNameCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int MsgId { get; set; }

        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelMemIndirectForUpdate";
        }

        public override bool IsValid()
        {
            return CustId > 0 && AgentId > 0;
        }
    }
}
