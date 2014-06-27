namespace LicenseeAgent.Transfer.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetTransferAgentDetailCriteria : CriteriaBase
    {
        public int AgentID { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Transfer_AgentDetail";
        }

        public override bool IsValid()
        {
            return AgentID > 0;
        }
    }
}
