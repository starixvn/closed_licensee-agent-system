namespace LicenseeAgent.WebService.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Criteria information.
    /// </summary>
    public class CheckAgentUsernameCriteria : CriteriaBase
    {
        public string SiteName { get; set; }

        public string AgentUsername { get; set; }

        public string MemberCurrency { get; set; }

        public int AgentId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Las_CheckAgentUsername";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(AgentUsername)
                && !string.IsNullOrEmpty(MemberCurrency)
                && !string.IsNullOrEmpty(SiteName);
        }
    }
}