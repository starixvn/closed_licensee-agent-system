namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// Event Entity.
    /// </summary>
    public class EventEntity
    {
        private string _matchName;

        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public int HomeTeam { get; set; }

        public int AwayTeam { get; set; }

        public bool IsLive { get; set; }

        public bool IsNeutral { get; set; }

        public string MatchName
        {
            get
            {
                if (_matchName == null)
                {
                    var neutral = IsNeutral ? " (N)" : string.Empty;
                    _matchName = DataRefWrapper.GetTeamName(HomeTeam) + neutral + " -vs- " + DataRefWrapper.GetTeamName(AwayTeam);
                }

                return _matchName;
            }

            set
            {
                _matchName = value;
            }
        }
    }
}