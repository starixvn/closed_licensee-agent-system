namespace LicenseeAgent.MemberArrangement.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Re-Arrangement Member Criteria.
    /// </summary>
    public class ReArrangementMemberCriteria : CriteriaBase
    {
        public int CurrencyId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string Ip { get; set; }

        public int LicenseeId { get; set; }

        public string MemberName { get; set; }

        public string NewAgentName { get; set; }

        public string OldAgentName { get; set; }

        public int UpdatedBy { get; set; }

        public override string GetSettingKey()
        {
            return "Las_Cash_ReArrangementMember";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(this.MemberName);
        }
    }
}