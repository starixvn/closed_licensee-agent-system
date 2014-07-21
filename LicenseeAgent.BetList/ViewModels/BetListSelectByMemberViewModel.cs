namespace LicenseeAgent.BetList.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// BetListSelectByMemberViewModel class.
    /// </summary>
    public class BetListSelectByMemberViewModel
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public string StatusIds { get; set; }

        public string ProductIds { get; set; }
    }
}