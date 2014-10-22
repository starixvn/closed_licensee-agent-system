namespace LicenseeAgent.CustomerList.Criteria.Agent
{
    using Fanex.Data.Repository;

    /// <summary>
    /// UpdateAgentStatusCriteria class.
    /// </summary>
    public class UpdateAgentStatusCriteria : CriteriaBase
    {
        public int Updater { get; set; }

        public int AgentId { get; set; }

        public int AccStatus { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_UpdateAgentAccStatus";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && Updater > 0;
        }
    }
}