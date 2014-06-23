// --------------------------------------------------------------------------
// <copyright file="WinLossMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using System;

    /// <summary>
    /// Win Loss Member Criteria.
    /// </summary>
    public class WinLossMemberCriteria : WinlossBase
    {
        public WinLossMemberCriteria(int agentId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay, DateTime balanceUpDay, short drawType, string productIds)
            : base(agentId, fromDate, toDate, moveBetsDay, balanceUpDay, drawType, productIds)
        {
        }

        public override string GetSettingKey()
        {
            return "LAS_Rpt_WinLoss_Member";
        }
    }
}