namespace LicenseeAgent.ProductSetting.Criteria.Keno
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Agent Edit Member Indirect Game Main Criteria class.
    /// </summary>
    public class AgentEditMemIndirectGameMainCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int ProductId { get; set; }

        public int AgentId { get; set; }

        public string PositionTaking3RD { get; set; }

        public int Updater { get; set; }

        public string Ip { get; set; }

        public string SettingOutput { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentEditMemIndirectGameMain";
        }

        public override bool IsValid()
        {
            return CustId > 0 && ProductId > 0 && AgentId > 0 && Updater > 0;
        }
    }
}
