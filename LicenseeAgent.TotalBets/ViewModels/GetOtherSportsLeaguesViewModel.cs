namespace LicenseeAgent.TotalBets.ViewModels
{
    /// <summary>
    /// GetOtherSportsLeaguesViewModel class.
    /// </summary>
    public class GetOtherSportsLeaguesViewModel
    {
        public GetOtherSportsLeaguesViewModel()
        {
            CheckedAll = true;
        }

        public string SelectedSports { get; set; }

        public string SelectedLeagues { get; set; }

        public bool CheckedAll { get; set; }
    }
}