// -----------------------------------------------------------------------
// <copyright file="SoccerDetailViewModel.cs" company="Nexcel Solutions Vietnam">
// SoccerDetailViewModel.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// Soccer Detail View Model.
    /// </summary>
    public class SoccerDetailViewModel
    {
        public int MatchId { get; set; }

        public string MatchName { get; set; }

        public string SelectedLeague { get; set; }

        public string SelectedEvent { get; set; }

        public bool CheckAllLeague { get; set; }

        public bool CheckAllEvent { get; set; }

        public int TimeInterval { get; set; }

        public IList<SoccerDetailEntity> MainData { get; set; }
    }
}