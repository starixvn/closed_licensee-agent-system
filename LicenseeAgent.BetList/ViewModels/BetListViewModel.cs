// ----------------------------------------------------------------------
// <copyright file="BetListViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.ViewModels
{
    using System.Collections.ObjectModel;
    using Entities;

    /// <summary>
    /// Bet List View Model.
    /// </summary>
    public class BetListViewModel
    {
        public Collection<TicketText> TicketTextList { get; set; }

        public TicketResult TicketResult { get; set; }

        public bool IsShowSystemIdAndExternalId { get; set; }

        public bool IsShowWinLoss { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
