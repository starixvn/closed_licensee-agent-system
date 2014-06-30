namespace LicenseeAgent.TotalBets.Entities
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// LeagueGroupEntity class.
    /// </summary>
    public class LeagueGroupEntity
    {
        private string _leagueName;

        public int LeagueId { get; set; }

        public bool LiveIndicator { get; set; }

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

        public IList<MatchEntity> MatchList { get; set; }
    }
}