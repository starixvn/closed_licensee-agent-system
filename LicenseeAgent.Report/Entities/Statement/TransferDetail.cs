namespace LicenseeAgent.Report.Entities.Statement
{
    using System;

    /// <summary>
    /// TransferDetail Entity.
    /// </summary>
    public class TransferDetail
    {
        public DateTime Date { get; set; }

        public string TransType { get; set; }

        public string TransDesc { get; set; }

        public decimal TransferAmt { get; set; }
    }
}