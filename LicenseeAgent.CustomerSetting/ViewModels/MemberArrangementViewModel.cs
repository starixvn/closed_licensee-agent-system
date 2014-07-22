namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CustomerSetting.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Member Arrangement View Model.
    /// </summary>
    public class MemberArrangementViewModel
    {
        [Display(Name = "Agent", ResourceType = typeof(Las))]
        public string AgentName { get; set; }

        [Display(Name = "Username", ResourceType = typeof(Las))]
        public string MemberName { get; set; }

        public ArrangementMode Mode { get; set; }

        [Display(Name = "NewAgent", ResourceType = typeof(Las))]
        public string NewAgentName { get; set; }

        [Display(Name = "OldAgent", ResourceType = typeof(Las))]
        public string OldAgentName { get; set; }
    }
}