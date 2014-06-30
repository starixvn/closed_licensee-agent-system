namespace LicenseeAgent.Account.ViewModels
{
    using LicenseeAgent.Account.Entities;

    /// <summary>
    /// HomePageViewModel class.
    /// </summary>
    public class HomePageViewModel
    {
        public StatisticEntity StatisticInfo { get; set; }

        public BalanceInfoEntity BalanceInfo { get; set; }
    }
}