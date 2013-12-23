// -----------------------------------------------------------------------
// <copyright file="ITotalBetsService.cs" company="Nexcel Solutions Vietnam">
// ITotalBetsService.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.TotalBets.Criterias;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// Total bet service.
    /// </summary>
    public interface ITotalBetsService
    {
        IEnumerable<LeagueEntity> GetLeagues(SoccerLeagueCriteria criteria);

        IEnumerable<EventEntity> GetEventsByLeague(SoccerEventCriteria criteria);

        IEnumerable<SoccerDataEntity> GetSoccerData(SoccerCriteria criteria);
    }
}