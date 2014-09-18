namespace LicenseeAgent.Customer.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Up line By Agent Id Criteria.
    /// </summary>
    public class GetUplineByAgentIdCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetUplineByAgentId";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
