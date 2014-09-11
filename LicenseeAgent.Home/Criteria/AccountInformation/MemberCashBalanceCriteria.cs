namespace LicenseeAgent.Home.Criteria.AccountInformation
{
    using Fanex.Data.Repository;

    /// <summary>
    /// MemberCashBalanceCriteria class.
    /// </summary>
    public class MemberCashBalanceCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal AvailableFund { get; set; }

        public decimal PendingFund { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Cash_Balance_SelMemberBalance";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}