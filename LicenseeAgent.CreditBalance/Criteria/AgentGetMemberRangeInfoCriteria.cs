// --------------------------------------------------------------------------
// <copyright file="AgentGetMemberRangeInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class AgentGetMemberRangeInfoCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal? MaxCreditOutput { get; set; }

        public decimal MinCreditOutput { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelMemberCredit";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}