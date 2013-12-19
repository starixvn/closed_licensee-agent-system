// -----------------------------------------------------------------------
// <copyright file="SoccerService.cs" company="Nexcel Solutions Vietnam">
// SoccerService.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Report.Criterias;
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
    }
}