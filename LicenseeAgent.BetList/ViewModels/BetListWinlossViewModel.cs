// -----------------------------------------------------------------------
// <copyright file="BetListWinlossViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.BetList.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListWinlossViewModel
    {
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string BreadCrumb { get; set; }

        public string DrawFilter { get; set; }

        public string StatusIDs { get; set; }
    }
}
