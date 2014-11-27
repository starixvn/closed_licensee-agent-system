namespace LicenseeAgent.BetList.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResultViewModel
    {
        [Required]
        public int SportType { get; set; }

        [Required]
        public int BetType { get; set; }

        public bool IsOutright { get; set; }

        public long? RefNo { get; set; }

        public string RaceNo { get; set; }

        public int? MatchId { get; set; }

        public string UserName { get; set; }

        public DateTime? WinlostDate { get; set; }

        public long? RefNoMixParlay { get; set; }

        public int? LeagueId { get; set; }

        public bool IsParlayPopup
        {
            get
            {
                return BetType == (int)LicenseeAgent.Core.Enumerations.BetType.BetType_9
                    || BetType == (int)LicenseeAgent.Core.Enumerations.BetType.BetType_29;
            }
        }
    }
}