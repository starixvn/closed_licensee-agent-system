namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Account.Criteria.HomePage;
    using LicenseeAgent.Account.Entities.HomePage;

    /// <summary>
    /// IHomePageService class.
    /// </summary>
    public interface IHomePageService
    {
        IList<TopWinningEntity> GetTopWinning(TopWinningCriteria criteria);

        IList<TopTurnoverEntity> GetTopTurnover(TopTurnoverCriteria criteria);

        IList<NewMemberOfMonthEntity> GetNewMemberOfMonth(NewMemberOfMonthCriteria criteria);

        StatisticInfo GetStatisticInfo(GetStatisticCriteria criteria);

        CreditBalanceInfo GetCreditBalanceInfo(GetCreditBalanceInfoCriteria criteria);

        CashBalanceInfo GetCashBalanceInfo(GetCashBalanceInfoCriteria criteria);

        IList<string> GetMemberSiteLinks(GetMemberSiteLinkCriteria criteria);
    }
}