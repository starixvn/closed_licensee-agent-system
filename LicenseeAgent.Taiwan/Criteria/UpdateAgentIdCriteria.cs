namespace LicenseeAgent.Taiwan.Criteria
{
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// EditInfoMemberCriteria class.
    /// </summary>
    public class UpdateAgentIdCriteria : SingleLogActionCriteria
    {
        public UpdateAgentIdCriteria(LogAction logAction)
            : base(logAction)
        {
        }

        public int CustId { get; set; }

        public int? NewLASAgentId { get; set; }

        public int? NewIBCAgentId { get; set; }

        public string AgentName { get; set; }

        public int LicenseeCurrencyId { get; set; }

        public int MemberCurrencyId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Switch_UpdateAgentId_Writelog";
        }

        public override bool IsValid()
        {
            return base.IsValid();
        }
    }
}