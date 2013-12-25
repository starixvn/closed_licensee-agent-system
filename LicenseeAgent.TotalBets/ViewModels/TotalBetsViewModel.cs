// -----------------------------------------------------------------------
// <copyright file="SoccerViewModel.cs" company="Nexcel Solutions Vietnam">
// SoccerViewModel.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SoccerViewModel
    {        
        public IEnumerable<LeagueEntity> LeagueList { get; set; }

        public IEnumerable<EventEntity> EventList { get; set; }

        public string League { get; set; }

        public string Event { get; set; }

        public int TimeInterval { get; set; }

        public IList<LeagueGroupEntity> MainData { get; set; }
    }
}