namespace LicenseeAgent.ProductSetting.Criteria.Keno
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentGetMemberSettingGameForUpdateCriteria class.
    /// </summary>
    public class AgentGetMemberSettingGameForUpdateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetMemberSettingGameForUpdate";
        }

        public override bool IsValid()
        {
            return CustId > 0 && ProductId > 0;
        }
    }
}
