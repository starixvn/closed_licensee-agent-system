// -----------------------------------------------------------------------
// <copyright file="SoccerService.cs" company="Nexcel Solutions Vietnam">
// SoccerService.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.TotalBets.Criterias;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.Report.Entities;

    /// <summary>
    /// Soccer Service.
    /// </summary>
    public class SoccerService : QueryService, ISoccerService
    {
        public IEnumerable<LeagueEntity> GetLeagues(SoccerLeagueCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IEnumerable<EventEntity> GetEventsByLeague(SoccerEventCriteria criteria)
        {
            return GetEntites<EventEntity>(criteria);
        }
    }
}