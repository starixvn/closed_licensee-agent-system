// -----------------------------------------------------------------------
// <copyright file="ISoccerService.cs" company="Nexcel Solutions Vietnam">
// ISoccerService.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Report.Criterias;
    using LicenseeAgent.Report.Entities;

    /// <summary>
    /// ISoccerService.
    /// </summary>
    public interface ISoccerService
    {
        IEnumerable<LeagueEntity> GetLeagues(SoccerLeagueCriteria criteria);
    }
}