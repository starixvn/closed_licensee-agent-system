namespace LicenseeAgent.ProductSetting.Criteria.VirtualSports
{
    using Fanex.Data.Repository;

    /// <summary>
    /// UpdateCustomerCriteria class.
    /// </summary>
    public class AgentUpdateAgentCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int Updater { get; set; }

        public string Ip { get; set; }

        public string PositionTaking3RD { get; set; }

        public string BetLimit3RD { get; set; }

        public string SettingOutput { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentEditAgentVirtualSports";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}