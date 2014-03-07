// -----------------------------------------------------------------------
// <copyright file="TotalBetsService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TotalBetsService : QueryService, ITotalBetsService
    {
        public IEnumerable<LeagueEntity> GetLeagues(LeagueCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IEnumerable<EventEntity> GetEventsByLeague(EventCriteria criteria)
        {
            return GetEntites<EventEntity>(criteria);
        }

        public IEnumerable<TotalBetsEntity> GetSoccerData(SoccerCriteria criteria)
        {
            return GetEntites<TotalBetsEntity>(criteria);
        }

        public IEnumerable<SoccerDetailEntity> GetSoccerDetailData(SoccerDetailCriteria criteria)
        {
            return GetEntites<SoccerDetailEntity>(criteria);
        }

        public IEnumerable<TotalBetsEntity> GetBasketballData(BasketballCriteria criteria)
        {
            return GetEntites<TotalBetsEntity>(criteria);
        }

        public IEnumerable<TotalBetsEntity> GetOtherSportsData(OtherSportsCriteria criteria)
        {
            return GetEntites<TotalBetsEntity>(criteria);
        }

        public IEnumerable<SportEntity> GetOtherSportsList(OtherSportsSportListCriteria criteria)
        {
            return GetEntites<SportEntity>(criteria);
        }

        public IEnumerable<LeagueEntity> GetOtherSportsLeagues(OtherSportsLeagueListCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IEnumerable<EventEntity> GetOtherSportsEventsByLeague(OtherSportsEventListCriteria criteria)
        {
            return GetEntites<EventEntity>(criteria);
        }

        public IEnumerable<SportEntity> GetOutrightSports(OutrightSportCriteria criteria)
        {
            return GetEntites<SportEntity>(criteria);
        }

        public IEnumerable<LeagueEntity> GetOutrightLeagues(OutrightLeagueCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IEnumerable<TeamEntity> GetOutrightTeam(OutrightTeamCriteria criteria)
        {
            return GetEntites<TeamEntity>(criteria);
        }

        public IEnumerable<OutrightEntity> GetOutrightData(OutrightCriteria criteria)
        {
            return GetEntites<OutrightEntity>(criteria);
        }
    }
}