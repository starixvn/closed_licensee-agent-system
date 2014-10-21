namespace LicenseeAgent.ProductSetting.Criteria.Keno
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentGetAgentSettingGameForUpdateCriteria class.
    /// </summary>
    public class AgentGetAgentSettingGameForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetAgentSettingGameForUpdate";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && ProductId > 0;
        }
    }
}
