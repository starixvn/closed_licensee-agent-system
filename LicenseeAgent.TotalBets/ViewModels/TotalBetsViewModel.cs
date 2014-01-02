// -----------------------------------------------------------------------
// <copyright file="TotalBetsViewModel.cs" company="Nexcel Solutions Vietnam">
// SoccerViewModel.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TotalBetsViewModel
    {
        [Required]
        public string LeagueList { get; set; }

        [Required]
        public string MatchList { get; set; }
    }
}