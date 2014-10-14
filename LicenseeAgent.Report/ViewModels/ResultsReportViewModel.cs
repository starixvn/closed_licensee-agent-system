namespace LicenseeAgent.Report.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// Bet List View Model.
    /// </summary>
    public class ResultsReportViewModel
    {
        public ResultsReportViewModel()
        {
            Today = DateTime.Today;
            Sports = new List<SelectListItem>();
            Leagues = new List<SelectListItem>();
        }

        public DateTime BalanceUpDay { get; set; }

        public DateTime Today { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public int SportType { get; set; }

        public IList<SelectListItem> Sports { get; set; }

        public IList<SelectListItem> Leagues { get; set; }
    }
}