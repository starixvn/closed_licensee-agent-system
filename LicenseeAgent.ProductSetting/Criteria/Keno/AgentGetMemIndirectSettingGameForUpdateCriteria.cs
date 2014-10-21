namespace LicenseeAgent.ProductSetting.Criteria.Keno
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Agent Get Member Indirect Setting Game For Update Criteria class.
    /// </summary>
    public class AgentGetMemIndirectSettingGameForUpdateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int AgentId { get; set; }

        public int ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetMemIndirectSettingGameForUpdate";
        }

        public override bool IsValid()
        {
            return CustId > 0 && AgentId > 0 && ProductId > 0;
        }
    }
}
