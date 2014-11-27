namespace LicenseeAgent.BetList.Criteria
{
    using System;

    /// <summary>
    /// BetListSelectByMemberCriteria class.
    /// </summary>
    public class SelectByMemberCriteria : BetListCriteriaBase
    {
        public string StatusIds { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string ProductIds { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Betlist_SelByMember";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && AgentId > 0 && CustId > 0;
        }
    }
}