namespace LicenseeAgent.CustomerList.ViewModels.PendingMember
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// PendingMemberListViewModel class.
    /// </summary>
    public class IndirectMemberViewModel
    {
        [Display(Name = "Member", ResourceType = typeof(Las))]
        public string IndirectMemberUserName { get; set; }

        [Display(Name = "Agent", ResourceType = typeof(Las))]
        public string AgentUserName { get; set; }
    }
}