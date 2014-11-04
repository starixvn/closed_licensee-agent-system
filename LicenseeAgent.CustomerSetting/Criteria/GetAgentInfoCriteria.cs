namespace LicenseeAgent.MemberReArrangement.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Agent Information Criteria.
    /// </summary>
    public class GetAgentInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetAgentInfoByAgentId";
        }

        public override bool IsValid()
        {
            return AgentId > 0 || !string.IsNullOrEmpty(Username);
        }
    }
}
