namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TotalBetsService class.
    /// </summary>
    public class TotalBetsService : QueryService, ITotalBetsService
    {
        public IList<LeagueEntity> GetLeagues(LeagueCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IList<EventEntity> GetEventsByLeague(EventCriteria criteria)
        {
            return GetEntites<EventEntity>(criteria);
        }

        public IList<TotalBetsEntity> GetSoccerData(SoccerCriteria criteria)
        {
            return GetEntites<TotalBetsEntity>(criteria);
        }

        public IList<SoccerDetailEntity> GetSoccerDetailData(SoccerDetailCriteria criteria)
        {
            return GetEntites<SoccerDetailEntity>(criteria);
        }

        public IList<TotalBetsEntity> GetBasketballData(BasketballCriteria criteria)
        {
            return GetEntites<TotalBetsEntity>(criteria);
        }

        public IList<TotalBetsEntity> GetOtherSportsData(OtherSportsCriteria criteria)
        {
            return GetEntites<TotalBetsEntity>(criteria);
        }

        public IList<SportEntity> GetOtherSportsList(OtherSportsSportListCriteria criteria)
        {
            return GetEntites<SportEntity>(criteria);
        }

        public IList<LeagueEntity> GetOtherSportsLeagues(OtherSportsLeagueListCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IList<EventEntity> GetOtherSportsEventsByLeague(OtherSportsEventListCriteria criteria)
        {
            return GetEntites<EventEntity>(criteria);
        }

        public IList<SportEntity> GetOutrightSports(OutrightSportCriteria criteria)
        {
            return GetEntites<SportEntity>(criteria);
        }

        public IList<LeagueEntity> GetOutrightLeagues(OutrightLeagueCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IList<TeamEntity> GetOutrightTeam(OutrightTeamCriteria criteria)
        {
            return GetEntites<TeamEntity>(criteria);
        }

        public IList<OutrightEntity> GetOutrightData(OutrightCriteria criteria)
        {
            return GetEntites<OutrightEntity>(criteria);
        }
    }
}