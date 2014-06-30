namespace LicenseeAgent.CustomerSetting.Criteria.NumberGame
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentGetAgentSettingForInsertCriteria class.
    /// </summary>
    public class AgentGetAgentSettingForInsertCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetAgentNumberGameSettingForInsert";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}