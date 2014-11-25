namespace LicenseeAgent.Report.ViewModels.Results
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// OutrightResultsViewModel class.
    /// </summary>
    public class OutrightResultsViewModel
    {
        public bool IsShowAll
        {
            get
            {
                return Constants.IsShowTestData;
            }
        }

        public DateTime BalanceUpDay { get; set; }

        [Required]
        [Display(Name = "From", ResourceType = typeof(Las))]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To", ResourceType = typeof(Las))]
        public DateTime ToDate { get; set; }

        [Display(Name = "Sport", ResourceType = typeof(Las))]
        public int Sport { get; set; }

        [Display(Name = "League", ResourceType = typeof(Las))]
        public int League { get; set; }
    }
}