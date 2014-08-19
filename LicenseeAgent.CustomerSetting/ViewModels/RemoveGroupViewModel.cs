namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// RemoveGroupViewModel class.
    /// </summary>
    public class RemoveGroupViewModel
    {
        [Required(ErrorMessageResourceName = "NoSelectedMember", ErrorMessageResourceType = typeof(Las))]
        public string MemberIds { get; set; }

        [Required]
        public bool SelectedConfirm { get; set; }
    }
}
