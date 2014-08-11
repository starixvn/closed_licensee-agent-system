namespace LicenseeAgent.Account.Criteria.HomePage
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// GetBalanceInfoCriteria class.
    /// </summary>
    public class GetBalanceInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public DateTime MoveBetDay { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_HomePage_SelBalanceInfo";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}