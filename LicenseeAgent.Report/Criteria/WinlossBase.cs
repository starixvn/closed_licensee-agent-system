// -----------------------------------------------------------------------
// <copyright file="WinlossBase.cs" company="Microsoft">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Win loss Report Base.
    /// </summary>
    public class WinlossBase : CriteriaBase
    {
        public WinlossBase(int agentId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay, DateTime balanceUpDay, short drawType)
        {
            AgentId = agentId;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDay = moveBetsDay;
            BalanceUpDay = balanceUpDay;
            DrawType = drawType;
        }

        public int AgentId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public short DrawType { get; set; }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
