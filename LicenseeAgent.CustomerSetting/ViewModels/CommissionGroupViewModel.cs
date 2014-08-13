namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Commission Group View Model class.
    /// </summary>
    public class CommissionGroupViewModel : CommissionGroupBaseViewModel
    {
        public int GroupCommId { get; set; }

        [Required(ErrorMessageResourceName = "GroupNameIsEmpty", ErrorMessageResourceType = typeof(Las))]
        public string GroupName { get; set; }

        public bool IsDefault { get; set; }

        public int SBComm1 { get; set; }

        public int SBComm2 { get; set; }

        public int SBComm3 { get; set; }

        public int RCComm1 { get; set; }

        public int RCComm2 { get; set; }

        public int RCComm3 { get; set; }

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