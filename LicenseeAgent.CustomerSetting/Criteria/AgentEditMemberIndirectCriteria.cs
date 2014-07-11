namespace LicenseeAgent.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentEditMemberIndirectCriteria class.
    /// </summary>
    public class AgentEditMemberIndirectCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int AgentId { get; set; }

        public int Updater { get; set; }

        public string PositionTaking { get; set; }

        public string PositionTaking3RD { get; set; }

        public string Ip { get; set; }

        public string SettingOutput { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentEditMemIndirect";
        }

        public override bool IsValid()
        {
            return CustId > 0 && AgentId > 0 && Updater > 0;
        }
    }
}