namespace LicenseeAgent.BetList.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// BetListTotalBetsViewModel class.
    /// </summary>
    public class TotalBetsViewModel
    {
        [Required]
        public int MatchId { get; set; }

        [Required]
        public int BetType { get; set; }

        [Required]
        public string BetTeam { get; set; }

        public bool? ShowLiveTicket { get; set; }
    }
}