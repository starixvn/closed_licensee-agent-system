namespace LicenseeAgent.Account.ViewModels.HomePage
{
    using System.Collections.Generic;
    using LicenseeAgent.Account.Entities.HomePage;

    /// <summary>
    /// HomePageViewModel class.
    /// </summary>
    public class HomePageCashViewModel
    {
        public StatisticInfo StatisticInfo { get; set; }

        public CashBalanceInfo BalanceInfo { get; set; }

        public IList<string> MemberSiteLinks { get; set; }
    }
}