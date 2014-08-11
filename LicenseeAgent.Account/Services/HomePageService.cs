namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using LicenseeAgent.Account.Criteria.HomePage;
    using LicenseeAgent.Account.Entities.HomePage;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// HomePageService class.
    /// </summary>
    public class HomePageService : QueryService, IHomePageService
    {
        public IList<TopWinningEntity> GetTopWinning(TopWinningCriteria criteria)
        {
            return GetEntites<TopWinningEntity>(criteria);
        }

        public IList<TopTurnoverEntity> GetTopTurnover(TopTurnoverCriteria criteria)
        {
            return GetEntites<TopTurnoverEntity>(criteria);
        }

        public IList<NewMemberOfMonthEntity> GetNewMemberOfMonth(NewMemberOfMonthCriteria criteria)
        {
            return GetEntites<NewMemberOfMonthEntity>(criteria);
        }

        public StatisticInfo GetStatisticInfo(GetStatisticCriteria criteria)
        {
            var list = GetEntites<StatisticInfo>(criteria);
            return list.FirstOrDefault();
        }

        public BalanceInfo GetBalanceInfo(GetBalanceInfoCriteria criteria)
        {
            var list = GetEntites<BalanceInfo>(criteria);
            return list.FirstOrDefault();
        }

        public BalanceCashInfo GetBalanceCashInfo(GetBalanceCashInfoCriteria criteria)
        {
            var list = GetEntites<BalanceCashInfo>(criteria);
            return list.FirstOrDefault();
        }

        public IList<string> GetMemberSiteLinks(GetMemberSiteLinkCriteria criteria)
        {
            var entities = GetEntites<string>(criteria);
            var result = new List<string>();
            foreach (var item in entities)
            {
                result.Add(item.Replace("{atag}", criteria.AgentUserName));
            }

            return result;
        }
    }
}