namespace LicenseeAgent.ProductSetting.Criteria.NumberGame
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LicenseeGetAgentSettingForInsertCriteria class.
    /// </summary>
    public class LicenseeGetAgentSettingForInsertCriteria : CriteriaBase
    {
        public int AgentL0Id { get; set; }

        public int MsgId { get; set; }

        public short MaxPTOfLicensee { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeGetAgentNumberGameSettingForInsert";
        }

        public override bool IsValid()
        {
            return AgentL0Id > 0;
        }
    }
}