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
        IEnumerable<LeagueEntity> GetLeagues(SoccerLeagueCriteria criteria);

        IEnumerable<EventEntity> GetEventsByLeague(SoccerEventCriteria criteria);

        IEnumerable<SoccerDataEntity> GetSoccerData(SoccerCriteria criteria);

        IEnumerable<SoccerDetailEntity> GetSoccerDetailData(SoccerDetailCriteria criteria);
    }
}