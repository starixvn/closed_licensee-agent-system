namespace LicenseeAgent.MemberArrangement.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Switch User Validate Criteria.
    /// </summary>
    public class SwitchUserValidateCriteria : CriteriaBase
    {
        public string MemberName { get; set; }

        public string AgentName { get; set; }

        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Las_Cash_SwitchUserValidate";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(MemberName);
        }
    }
}