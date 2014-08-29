namespace LicenseeAgent.AccountInformation.Entities
{
    using System;

    /// <summary>
    /// BalanceEntity class.
    /// </summary>
    public class BalanceEntity
    {
        public decimal TotalCash { get; set; }

        public decimal YesterdayCash { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal TotalOutstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal UsedCredit { get; set; }

        public decimal BetCredit { get; set; }

        public decimal CashBalance { get; set; }

        public decimal AvailableFund { get; set; }

        public decimal PendingFund { get; set; }

        public DateTime Today { get; set; }
    }
}