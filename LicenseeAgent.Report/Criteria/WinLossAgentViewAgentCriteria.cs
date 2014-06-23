// --------------------------------------------------------------------------
// <copyright file="WinLossAgentViewAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using System;

    /// <summary>
    /// Win Loss Agent View Agent Criteria.
    /// </summary>
    public class WinLossAgentViewAgentCriteria : WinlossBase
    {
        public WinLossAgentViewAgentCriteria(
            int agentId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            DateTime balanceUpDay,
            short drawType,
            string productIds)
            : base(agentId, fromDate, toDate, moveBetsDay, balanceUpDay, drawType, productIds)
        {
        }

        public override string GetSettingKey()
        {
            return "LAS_Rpt_WinLoss_AgentViewAgent";
        }
    }
}