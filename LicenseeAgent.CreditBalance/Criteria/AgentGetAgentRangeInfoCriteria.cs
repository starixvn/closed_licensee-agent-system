// --------------------------------------------------------------------------
// <copyright file="AgentGetAgentRangeInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// AgentGetAgentRangeInfoCriteria class.
    /// </summary>
    public class AgentGetAgentRangeInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public decimal? MaxCreditOutput { get; set; }

        public decimal MinCreditOutput { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelAgentCredit";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}