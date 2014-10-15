namespace LicenseeAgent.Home.Criteria.Account
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentUserLogoutCriteria class.
    /// </summary>
    public class AgentUserLogoutCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "On_AgentId")]
        public int OnAgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentUserLogout";
        }

        public override bool IsValid()
        {
            return OnAgentId > 0;
        }
    }
}