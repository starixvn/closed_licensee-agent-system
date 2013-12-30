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
        public string MatchName { get; set; }

        public IList<SoccerDetailEntity> MainData { get; set; }
    }
}