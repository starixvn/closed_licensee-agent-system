namespace LicenseeAgent.AccountInformation.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentCashBalanceCriteria class.
    /// </summary>
    public class AgentCashBalanceCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public DateTime MoveBetDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public decimal Balance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal TotalOutstanding { get; set; }

        public DateTime Today { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Cash_Balance_SelAgentBalance";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
