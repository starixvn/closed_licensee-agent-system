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
        public string MemberUserName { get; set; }

        [Display(Name = "DirectAgent", ResourceType = typeof(Las))]
        [Required(ErrorMessageResourceName = "Msg_2110", ErrorMessageResourceType = typeof(LangMsg))]
        public string AgentUserName { get; set; }
    }
}