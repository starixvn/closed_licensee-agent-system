// -----------------------------------------------------------------------
// <copyright file="SoccerViewModel.cs" company="Nexcel Solutions Vietnam">
// SoccerViewModel.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.ViewModels
{
    public class SoccerViewModel
    {
        public int TimeInterval { get; set; }

        public string SelectedLeague { get; set; }

        public string SelectedEvent { get; set; }

        public bool CheckAllLeague { get; set; }

        public bool CheckAllEvent { get; set; }
    }
}