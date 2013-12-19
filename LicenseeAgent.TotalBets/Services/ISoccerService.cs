// --------------------------------------------------------------------------
// <copyright file="ISoccerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.TotalBets.Criterias;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// ISoccer Service.
    /// </summary>
    public interface ISoccerService
    {
        IEnumerable<LeagueEntity> GetLeagues(SoccerLeagueCriteria criteria);

        IEnumerable<EventEntity> GetEventsByLeague(SoccerEventCriteria criteria);
    }
}