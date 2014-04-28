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
        IList<LeagueEntity> GetLeagues(LeagueCriteria criteria);

        IList<EventEntity> GetEventsByLeague(EventCriteria criteria);

        IList<TotalBetsEntity> GetSoccerData(SoccerCriteria criteria);

        IList<SoccerDetailEntity> GetSoccerDetailData(SoccerDetailCriteria criteria);

        IList<TotalBetsEntity> GetBasketballData(BasketballCriteria criteria);

        IList<TotalBetsEntity> GetOtherSportsData(OtherSportsCriteria criteria);

        IList<SportEntity> GetOtherSportsList(OtherSportsSportListCriteria criteria);

        IList<LeagueEntity> GetOtherSportsLeagues(OtherSportsLeagueListCriteria criteria);

        IList<EventEntity> GetOtherSportsEventsByLeague(OtherSportsEventListCriteria criteria);

        IList<SportEntity> GetOutrightSports(OutrightSportCriteria criteria);

        IList<LeagueEntity> GetOutrightLeagues(OutrightLeagueCriteria criteria);

        IList<TeamEntity> GetOutrightTeam(OutrightTeamCriteria criteria);

        IList<OutrightEntity> GetOutrightData(OutrightCriteria criteria);
    }
}