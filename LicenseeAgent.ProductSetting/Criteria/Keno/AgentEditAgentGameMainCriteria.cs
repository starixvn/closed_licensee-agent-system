namespace LicenseeAgent.ProductSetting.Criteria.Keno
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentEditAgentGameMainCriteria class.
    /// </summary>
    public class AgentEditAgentGameMainCriteria : CriteriaBase
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
            return "Las_AgentEditAgentGameMain";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && ProductId > 0 && Updater > 0;
        }
    }
}
