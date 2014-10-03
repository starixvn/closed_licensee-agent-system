namespace LicenseeAgent.CustomerList.ViewModels.PendingMember
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// PendingMemberListViewModel class.
    /// </summary>
    public class DirectMemberViewModel
    {
        [Display(Name = "Member", ResourceType = typeof(Las))]
        public string MemberUserName { get; set; }
    }
}