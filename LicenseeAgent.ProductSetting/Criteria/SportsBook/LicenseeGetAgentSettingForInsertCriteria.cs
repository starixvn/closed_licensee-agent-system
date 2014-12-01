namespace LicenseeAgent.ProductSetting.Criteria.SportsBook
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LicenseeGetAgentSettingForInsertCriteria class.
    /// </summary>
    public class LicenseeGetAgentSettingForInsertCriteria : CriteriaBase
    {
        public int AgentL0Id { get; set; }

        public decimal MaxCredit { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public short MaxPTOfLicensee { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeGetAgentSportsBookSettingForInsert";
        }

        public override bool IsValid()
        {
            return AgentL0Id > 0;
        }
    }
}