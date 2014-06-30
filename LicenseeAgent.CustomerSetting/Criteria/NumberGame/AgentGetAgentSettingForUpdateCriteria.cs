namespace LicenseeAgent.CustomerSetting.Criteria.NumberGame
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Agent Position Taking criteria.
    /// </summary>
    public class AgentGetAgentSettingForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Las_AgentGetAgentNumberGameSettingForUpdate";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}