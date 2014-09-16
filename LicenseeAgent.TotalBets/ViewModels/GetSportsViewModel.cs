namespace LicenseeAgent.TotalBets.ViewModels
{
    /// <summary>
    /// GetSportsViewModel class.
    /// </summary>
    public class GetSportsViewModel
    {
        public GetSportsViewModel()
        {
            CheckedAll = true;
        }

        public string SelectedSports { get; set; }

        public bool CheckedAll { get; set; }
    }
}