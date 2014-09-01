namespace LicenseeAgent.AccountInformation.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// CashMemberBalanceCriteria class.
    /// </summary>
    public class CashMemberBalanceCriteria : CriteriaBase
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
