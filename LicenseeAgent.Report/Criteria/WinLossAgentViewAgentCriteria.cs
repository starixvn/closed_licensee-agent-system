// --------------------------------------------------------------------------
// <copyright file="WinLossAgentViewAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.2.12">Created</change>
// </history>

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Win Loss Agent View Agent Criteria.
    /// </summary>
    public class WinLossAgentViewAgentCriteria : WinlossBase
    {
        public WinLossAgentViewAgentCriteria(int agentId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay, DateTime balanceUpDay, short drawType) 
            : base(agentId, fromDate, toDate, moveBetsDay, balanceUpDay, drawType) 
        {
        }

        public override string GetSettingKey()
        {
            return "LAS_Rpt_WinLoss_AgentViewAgent";
        }
    }
}
