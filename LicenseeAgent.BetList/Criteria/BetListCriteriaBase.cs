namespace LicenseeAgent.BetList.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// BetListCriteriaBase class.
    /// </summary>
    public abstract class BetListCriteriaBase : CriteriaBase
    {
        public int AgentId { get; set; }

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}