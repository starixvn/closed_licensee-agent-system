namespace LicenseeAgent.CommissionSetting.ViewModels.MemberList
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CommissionSetting.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// RemoveGroupViewModel class.
    /// </summary>
    public class RemoveFromGroupViewModel
    {
        [Required(ErrorMessageResourceName = "NoSelectedMember", ErrorMessageResourceType = typeof(Las))]
        public string CustIds { get; set; }

        [Required]
        public DeleteMode Mode { get; set; }
    }
}