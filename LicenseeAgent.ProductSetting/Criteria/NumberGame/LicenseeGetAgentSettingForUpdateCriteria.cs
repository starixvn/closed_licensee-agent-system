namespace LicenseeAgent.ProductSetting.Criteria.NumberGame
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LicenseeGetAgentSettingForUpdateCriteria class.
    /// </summary>
    public class LicenseeGetAgentSettingForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeGetAgentNumberGameSettingForUpdate";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}