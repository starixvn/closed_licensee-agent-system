namespace LicenseeAgent.Core.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetAgentInfoCriteria class.
    /// </summary>
    public class GetAgentInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetAgentInfoById";
        }

        public override bool IsValid()
        {
            return AgentId != 0;
        }
    }
}
