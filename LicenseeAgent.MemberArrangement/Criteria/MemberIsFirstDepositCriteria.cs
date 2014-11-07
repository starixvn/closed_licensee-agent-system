namespace LicenseeAgent.MemberArrangement.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Member Is First Deposit Criteria.
    /// </summary>
    public class MemberIsFirstDepositCriteria : CriteriaBase
    {
        public bool HasFirstDeposit { get; set; }

        public int LicenseeId { get; set; }

        public string MemberName { get; set; }

        public override string GetSettingKey()
        {
            return "Las_Cash_HasFirstDeposit";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(this.MemberName);
        }
    }
}