namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// League Entity.
    /// </summary>
    public class LeagueEntity
    {
        private string _leagueName;

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return _leagueName ?? (_leagueName = DataRefWrapper.GetLeagueName(LeagueId));
            }

            set
            {
                _leagueName = value;
            }
        }
    }
}