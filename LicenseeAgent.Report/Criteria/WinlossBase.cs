namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Win loss Report Base.
    /// </summary>
    public abstract class WinlossBase : CriteriaBase
    {
        protected WinlossBase(int agentId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay, DateTime balanceUpDay, short drawType, string productIds)
        {
            AgentId = agentId;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDay = moveBetsDay;
            BalanceUpDay = balanceUpDay;
            DrawType = drawType;
            ProductIds = productIds;
        }

        public int AgentId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public short DrawType { get; set; }

        public string ProductIds { get; set; }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}