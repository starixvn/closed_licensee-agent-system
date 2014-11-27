namespace LicenseeAgent.BetList.ViewModels
{
    using System;

    /// <summary>
    /// BetListWinLossViewModel class.
    /// </summary>
    public class WinLossViewModel
    {
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string BreadCrumb { get; set; }

        public string DrawFilter { get; set; }

        public string Status { get; set; }

        public string Product { get; set; }

        public string Title { get; set; }
    }
}