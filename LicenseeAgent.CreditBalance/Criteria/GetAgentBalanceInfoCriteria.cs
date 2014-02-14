// --------------------------------------------------------------------------
// <copyright file="GetAgentBalanceInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetAgentBalanceInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public int? Status { get; set; }

        public bool IsYBFiltered { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_CreditBalance_AgentBalanceInfo";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}