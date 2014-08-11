namespace LicenseeAgent.Account.Entities.HomePage
{
    using System;

    /// <summary>
    /// BalanceInfoCashEntity class.
    /// </summary>
    public class BalanceCashInfo
    {
        public string AgentName { get; set; }

        public string CurrencyName { get; set; }

        public decimal Balance { get; set; }

        public decimal TodayWinloss { get; set; }

        public decimal YesterdayWinloss { get; set; }

        public DateTime Today { get; set; }

        public DateTime Yesterday { get; set; }
    }
}