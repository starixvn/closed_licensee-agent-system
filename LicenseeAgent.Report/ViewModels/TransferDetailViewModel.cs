namespace LicenseeAgent.Report.ViewModels
{
    using System;

    /// <summary>
    /// Statement Report View Model.
    /// </summary>
    public class TransferDetailViewModel
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsMember { get; set; }
    }
}