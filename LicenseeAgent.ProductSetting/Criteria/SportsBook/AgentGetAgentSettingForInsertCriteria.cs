namespace LicenseeAgent.ProductSetting.Criteria.SportsBook
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Agent setting for insert new agent.
    /// </summary>
    public class AgentGetAgentSettingForInsertCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MsgId { get; set; }

        public decimal MaxCredit { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetAgentSportsBookSettingForInsert";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}