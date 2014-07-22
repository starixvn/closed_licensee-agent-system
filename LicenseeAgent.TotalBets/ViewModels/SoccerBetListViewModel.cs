namespace LicenseeAgent.TotalBets.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// SoccerBetListViewResult class.
    /// </summary>
    public class SoccerBetListViewModel
    {
        [Required]
        public int MatchId { get; set; }

        [Required]
        public int BetType { get; set; }

        [Required]
        public string BetTeam { get; set; }
    }
}