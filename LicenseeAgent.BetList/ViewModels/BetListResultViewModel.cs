// -----------------------------------------------------------------------
// <copyright file="BetListResultViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.BetList.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListResultViewModel
    {
        [Required]
        public int SportType { get; set; }

        [Required]
        public int BetType { get; set; }

        public bool IsOutright { get; set; }

        public long? RefNo { get; set; }

        public long? RaceNo { get; set; }

        public int? MatchId { get; set; }

        public string UserName { get; set; }

        public DateTime? WinlostDate { get; set; }

        public long? RefNoMixParlay { get; set; }

        public int? LeagueId { get; set; }
    }
}