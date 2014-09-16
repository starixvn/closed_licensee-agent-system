namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TotalBetsService class.
    /// </summary>
    public class OtherSportsService : QueryService, IOtherSportsService
    {
        public IList<SportEntity> GetOtherSports(OtherSportsSportListCriteria criteria)
        {
            return GetEntites<SportEntity>(criteria);
        }

        public IList<TotalBetsEntity> GetOtherSportsData(OtherSportsCriteria criteria)
        {
            return GetEntites<TotalBetsEntity>(criteria);
        }

        public IList<LeagueEntity> GetOtherSportsLeagues(OtherSportsLeagueListCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IList<EventEntity> GetOtherSportsEventsByLeague(OtherSportsEventListCriteria criteria)
        {
            return GetEntites<EventEntity>(criteria);
        }
    }
}