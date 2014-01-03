// -----------------------------------------------------------------------
// <copyright file="OutrightViewModel.cs" company="Nexcel Solutions Vietnam">
// OutrightViewModel.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutrightViewModel
    {        
        public IEnumerable<LeagueEntity> LeagueList { get; set; }

        public IEnumerable<SportEntity> SportList { get; set; }

        public IEnumerable<TeamEntity> TeamList { get; set; }

        public string League { get; set; }

        public string Sport { get; set; }

        public string Team { get; set; }

        public int TimeInterval { get; set; }        
    }
}