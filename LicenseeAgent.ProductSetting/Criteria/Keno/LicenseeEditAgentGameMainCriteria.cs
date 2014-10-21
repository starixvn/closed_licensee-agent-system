namespace LicenseeAgent.ProductSetting.Criteria.Keno
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LicenseeEditAgentGameMainCriteria class.
    /// </summary>
    public class LicenseeEditAgentGameMainCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int ProductId { get; set; }

        public int Updater { get; set; }

        public string Ip { get; set; }

        public string PositionTaking3RD { get; set; }

        public string BetLimit3RD { get; set; }

        public string SettingOutput { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeEditAgentGameMain";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && ProductId > 0 && Updater > 0;
        }
    }
}
