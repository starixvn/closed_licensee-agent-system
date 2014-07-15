namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using LicenseeAgent.CustomerSetting.Enumerations;

    /// <summary>
    /// Member Arrangement View Model.
    /// </summary>
    public class MemberArrangementViewModel
    {
        public string AgentName { get; set; }

        public string MemberName { get; set; }

        public ArrangementMode Mode { get; set; }

        public string NewAgentName { get; set; }

        public string OldAgentName { get; set; }
    }
}