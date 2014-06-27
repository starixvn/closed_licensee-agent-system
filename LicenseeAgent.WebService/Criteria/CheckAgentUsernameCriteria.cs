namespace LicenseeAgent.WebService.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Criteria information.
    /// </summary>
    public class CheckAgentUsernameCriteria : CriteriaBase
    {
        public string AgentUsername { get; set; }

        public string SiteName { get; set; }

        public int AgentId { get; set; }

        public string AgentCurrency { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Las_CheckAgentUsername";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(AgentUsername)
                && !string.IsNullOrEmpty(SiteName);
        }
    }
}