// --------------------------------------------------------------------------
// <copyright file="GetAgentBalanceInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetAgentBalanceInfoCriteria class.
    /// </summary>
    public class GetAgentBalanceInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public int? Status { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Transfer_AgentBalanceInfo";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}