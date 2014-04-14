// --------------------------------------------------------------------------
// <copyright file="GetMemberBalanceInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class GetMemberBalanceInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Transfer_MemberBalanceInfo";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}