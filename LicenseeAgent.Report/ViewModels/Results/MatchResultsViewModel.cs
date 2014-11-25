namespace LicenseeAgent.Report.ViewModels.Results
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// MatchResultsViewModel class.
    /// </summary>
    public class MatchResultsViewModel
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
        public DateTime Date { get; set; }

        [Display(Name = "Sport", ResourceType = typeof(Las))]
        public int Sport { get; set; }

        [Display(Name = "League", ResourceType = typeof(Las))]
        public int League { get; set; }
    }
}