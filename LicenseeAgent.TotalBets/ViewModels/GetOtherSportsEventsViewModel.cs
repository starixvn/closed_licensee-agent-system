namespace LicenseeAgent.TotalBets.ViewModels
{
    /// <summary>
    /// GetOtherSportsEventsViewModel class.
    /// </summary>
    public class GetOtherSportsEventsViewModel
    {
        public GetOtherSportsEventsViewModel()
        {
            CheckedAll = true;
        }

        public string SelectedSports { get; set; }

        public string SelectedLeagues { get; set; }

        public string SelectedEvents { get; set; }

        public bool CheckedAll { get; set; }
    }
}