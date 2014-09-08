namespace LicenseeAgent.Account.Entities.HomePage
{
    using System;

    /// <summary>
    /// BalanceInfoEntity class.
    /// </summary>
    public class CreditBalanceInfo
    {
        public string AgentName { get; set; }

        public string CurrencyName { get; set; }

        public decimal TotalCash { get; set; }

        public decimal YesterdayCash { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal TodayWinloss { get; set; }

        public decimal YesterdayWinloss { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal UsedCredit { get; set; }

        public DateTime Today { get; set; }

        public DateTime Yesterday { get; set; }
    }
}