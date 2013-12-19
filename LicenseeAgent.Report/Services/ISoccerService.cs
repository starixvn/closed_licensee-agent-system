// --------------------------------------------------------------------------
// <copyright file="ISoccerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Report.Criterias;
    using LicenseeAgent.Report.Entities;

    /// <summary>
    /// Interface Soccer Service.
    /// </summary>
    public interface ISoccerService
    {
        IEnumerable<LeagueEntity> GetLeagues(SoccerLeagueCriteria criteria);
    }
}