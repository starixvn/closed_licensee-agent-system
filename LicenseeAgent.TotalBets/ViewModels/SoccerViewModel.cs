namespace LicenseeAgent.TotalBets.ViewModels
{
    using LicenseeAgent.TotalBets.Core;

    /// <summary>
    /// SoccerViewModel class.
    /// </summary>
    public class SoccerViewModel
    {
        public SoccerViewModel()
        {
            TimeInterval = Constant.RefreshCountDownInterval;
            CheckAllLeague = true;
            CheckAllEvent = true;
        }

        public int TimeInterval { get; set; }

        public string SelectedLeagues { get; set; }

        public bool CheckAllLeague { get; set; }

        public string SelectedEvents { get; set; }

        public bool CheckAllEvent { get; set; }
    }
}