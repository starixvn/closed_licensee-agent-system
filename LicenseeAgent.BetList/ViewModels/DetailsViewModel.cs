namespace LicenseeAgent.BetList.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.BetList.Entities;

    /// <summary>
    /// DetailsViewModel class.
    /// </summary>
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            TicketDetails = new List<TicketDetail>();
        }

        [Required]
        public long TransId { get; set; }

        [Required]
        public int SportType { get; set; }

        [Required]
        public int BetType { get; set; }

        [Required]
        public string Status { get; set; }

        public IList<TicketDetail> TicketDetails { get; set; }

        public bool IsDeposit { get; set; }
    }
}