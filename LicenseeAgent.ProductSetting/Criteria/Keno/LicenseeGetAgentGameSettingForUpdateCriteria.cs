namespace LicenseeAgent.ProductSetting.Criteria.Keno
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LicenseeGetAgentGameSettingForUpdateCriteria class.
    /// </summary>
    public class LicenseeGetAgentGameSettingForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeGetAgentGameSettingForUpdate";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && ProductId > 0;
        }
    }
}
