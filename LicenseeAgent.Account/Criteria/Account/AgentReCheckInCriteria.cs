namespace LicenseeAgent.Account.Criteria.Account
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentReCheckInCriteria class.
    /// </summary>
    public class AgentReCheckInCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "On_AgentId")]
        public int OnAgentId { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentReCheckIn";
        }

        public override bool IsValid()
        {
            return OnAgentId > 0;
        }
    }
}