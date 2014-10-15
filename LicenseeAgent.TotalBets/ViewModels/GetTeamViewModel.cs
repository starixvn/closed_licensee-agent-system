namespace LicenseeAgent.TotalBets.ViewModels
{
    /// <summary>
    /// GetTeamViewModel class.
    /// </summary>
    public class GetTeamViewModel
    {
        public GetTeamViewModel()
        {
            CheckedAll = true;
        }

        public string SelectedSports { get; set; }

        public string SelectedLeagues { get; set; }

        public string SelectedTeams { get; set; }

        public bool CheckedAll { get; set; }
    }
}