namespace LicenseeAgent.MemberReArrangement.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Switch User From Normal To Legend Criteria.
    /// </summary>
    public class SwitchUserFromNormalToLegendCriteria : CriteriaBase
    {
        public string AgentName { get; set; }

        public int CurrencyId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string Ip { get; set; }

        public int LicenseeId { get; set; }

        public string MemberName { get; set; }

        public int UpdatedBy { get; set; }

        public override string GetSettingKey()
        {
            return "Las_Cash_SwitchUserFromNormalToLegend";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(this.MemberName);
        }
    }
}