// --------------------------------------------------------------------------
// <copyright file="WinLossSuperViewAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using System;

    /// <summary>
    /// Win Loss Super View Agent Criteria.
    /// </summary>
    public class WinLossSuperViewAgentCriteria : WinlossBase
    {
        public WinLossSuperViewAgentCriteria(
            int agentId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            DateTime balanceUpDay,
            short drawType) 
            : base(agentId, fromDate, toDate, moveBetsDay, balanceUpDay, drawType)
        { 
        }

        public override string GetSettingKey()
        {
            return "LAS_Rpt_WinLoss_SuperViewAgent";
        }
    }
}
