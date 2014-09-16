namespace LicenseeAgent.TotalBets.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// GetSoccerLeagues class.
    /// </summary>
    public class GetSportLeaguesViewModel
    {
        public GetSportLeaguesViewModel()
        {
            CheckedAll = true;
        }

        [Required]
        public int SportType { get; set; }

        public string SelectedLeagues { get; set; }

        public bool CheckedAll { get; set; }
    }
}