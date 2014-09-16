namespace LicenseeAgent.TotalBets.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// GetSoccerEventsViewModel class.
    /// </summary>
    public class GetSportEventsViewModel
    {
        public GetSportEventsViewModel()
        {
            CheckedAll = true;
        }

        [Required]
        public int SportType { get; set; }

        public string SelectedLeagues { get; set; }

        public string SelectedEvents { get; set; }

        public bool CheckedAll { get; set; }
    }
}