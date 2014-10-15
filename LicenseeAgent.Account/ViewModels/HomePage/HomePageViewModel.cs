namespace LicenseeAgent.Account.ViewModels.HomePage
{
    using LicenseeAgent.Account.Entities.HomePage;

    /// <summary>
    /// HomePageViewModel class.
    /// </summary>
    public class HomePageViewModel
    {
        public StatisticInfo StatisticInfo { get; set; }

        public CreditBalanceInfo BalanceInfo { get; set; }
    }
}