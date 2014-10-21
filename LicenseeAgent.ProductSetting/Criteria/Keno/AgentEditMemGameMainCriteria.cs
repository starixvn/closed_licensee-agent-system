namespace LicenseeAgent.ProductSetting.Criteria.Keno
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Agent Edit Member Game Main Criteria class.
    /// </summary>
    public class AgentEditMemGameMainCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int ProductId { get; set; }

        public int Updater { get; set; }

        public string Ip { get; set; }

        public string PositionTaking3RD { get; set; }

        public string BetLimit3RD { get; set; }

        public string SettingOutput { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentEditMemGameMain";
        }

        public override bool IsValid()
        {
            return CustId > 0 && ProductId > 0 && Updater > 0;
        }
    }
}
