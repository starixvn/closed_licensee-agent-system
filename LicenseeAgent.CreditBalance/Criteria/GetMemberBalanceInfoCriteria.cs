// --------------------------------------------------------------------------
// <copyright file="GetMemberBalanceInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class GetMemberBalanceInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public bool IsYBFiltered { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_CreditBalance_MemberBalanceInfo";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}