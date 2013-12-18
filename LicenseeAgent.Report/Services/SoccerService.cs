// -----------------------------------------------------------------------
// <copyright file="SoccerService.cs" company="Nexcel Solutions Vietnam">
// SoccerService.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Report.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Report.Entities;
    using LicenseeAgent.Report.Criterias;

    /// <summary>
    /// SoccerService.
    /// </summary>
    public class SoccerService : QueryService, ISoccerService
    {
        public IEnumerable<LeagueEntity> GetLeagues(SoccerLeagueCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }
    }
}
