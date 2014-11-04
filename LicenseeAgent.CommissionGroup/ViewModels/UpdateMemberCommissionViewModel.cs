namespace LicenseeAgent.CommissionGroup.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// UpdateMemberCommissionViewModel class.
    /// </summary>
    public class UpdateMemberCommissionViewModel : CommissionGroupBaseViewModel
    {
        [Required(ErrorMessageResourceName = "NoSelectedMember", ErrorMessageResourceType = typeof(Las))]
        public string MemberIds { get; set; }

        [Required]
        public int SBComm1 { get; set; }

        [Required]
        public int SBComm2 { get; set; }

        [Required]
        public int SBComm3 { get; set; }

        public int RacingComm1 { get; set; }

        public int RacingComm2 { get; set; }

        public int RacingComm3 { get; set; }

        public int NGComm1 { get; set; }

        public int NGComm2 { get; set; }

        public int NGComm3 { get; set; }

        public int LCComm1 { get; set; }

        public int LCComm2 { get; set; }

        public int LCComm3 { get; set; }

        public int RNGComm1 { get; set; }

        public int RNGComm2 { get; set; }

        public int RNGComm3 { get; set; }
    }
}
