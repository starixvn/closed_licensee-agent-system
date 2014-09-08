namespace LicenseeAgent.AccountInformation.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// MemberBalanceCriteria class.
    /// </summary>
    public class MemberCreditBalanceCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal BetCredit { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Balance_SelMemberBalance";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}