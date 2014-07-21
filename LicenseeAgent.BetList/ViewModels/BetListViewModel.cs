namespace LicenseeAgent.BetList.ViewModels
{
    using System.Collections.Generic;
    using Entities;

    /// <summary>
    /// Bet List View Model.
    /// </summary>
    public class BetListViewModel
    {
        public IList<TicketText> TicketTextList { get; set; }

        public TicketResult TicketResult { get; set; }

        public bool IsShowSystemIdAndExternalId { get; set; }

        public bool IsShowWinLoss { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}