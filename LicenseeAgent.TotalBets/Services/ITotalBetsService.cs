// -----------------------------------------------------------------------
// <copyright file="ITotalBetsService.cs" company="Nexcel Solutions Vietnam">
// ITotalBetsService.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// Total bet service.
    /// </summary>
    public interface ITotalBetsService
    {
        IEnumerable<LeagueEntity> GetLeagues(LeagueCriteria criteria);

        IEnumerable<EventEntity> GetEventsByLeague(EventCriteria criteria);

        IEnumerable<TotalBetsEntity> GetSoccerData(SoccerCriteria criteria);

        IEnumerable<SoccerDetailEntity> GetSoccerDetailData(SoccerDetailCriteria criteria);

        IEnumerable<TotalBetsEntity> GetBasketballData(BasketballCriteria criteria);

        IEnumerable<TotalBetsEntity> GetOtherSportsData(OtherSportsCriteria criteria);

        IEnumerable<SportEntity> GetOtherSportsList(OtherSportsSportListCriteria criteria);

        IEnumerable<LeagueEntity> GetOtherSportsLeagues(OtherSportsLeagueListCriteria criteria);

        IEnumerable<EventEntity> GetOtherSportsEventsByLeague(OtherSportsEventListCriteria criteria);
    }
}