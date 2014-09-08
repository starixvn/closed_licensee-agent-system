namespace LicenseeAgent.Account.Criteria.HomePage
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// BalanceInfoCashCriteria class.
    /// </summary>
    public class GetCashBalanceInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public DateTime MoveBetDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_HomePage_SelBalanceInfo_Cash";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}