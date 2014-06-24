// --------------------------------------------------------------------------
// <copyright file="StatementAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Statement Agent Criteria.
    /// </summary>
    public class StatementAgentCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Statement_SelDownlineAgents";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}