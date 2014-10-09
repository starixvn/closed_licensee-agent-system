namespace LicenseeAgent.BetList.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Bet List View Model.
    /// </summary>
    public class BetListViewModel
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Product { get; set; }
    }
}