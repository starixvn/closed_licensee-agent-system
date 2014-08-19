namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// UpdateMemberCommissionViewModel class.
    /// </summary>
    public class UpdateMemberCommissionViewModel
    {
        [Required(ErrorMessageResourceName = "NoSelectedMember", ErrorMessageResourceType = typeof(Las))]
        public string MemberIds { get; set; }

        [Required]
        public int SBComm1 { get; set; }

        [Required]
        public int SBComm2 { get; set; }

        [Required]
        public int SBComm3 { get; set; }

        [Required]
        public int NGComm1 { get; set; }

        [Required]
        public int NGComm2 { get; set; }

        [Required]
        public int NGComm3 { get; set; }
    }
}
