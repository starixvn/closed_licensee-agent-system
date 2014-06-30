namespace LicenseeAgent.Transfer.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Transfer Agent Detail Criteria.
    /// </summary>
    public class GetTransferAgentDetailCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Transfer_AgentDetail";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}