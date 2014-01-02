// -----------------------------------------------------------------------
// <copyright file="EventEntity.cs" company="Nexcel Solutions Vietnam">
// EventEntity.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// Event Entity.
    /// </summary>
    public class EventEntity
    {
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
                var neutral = IsNeutral ? " (N)" : string.Empty;
                var matchName = DataRefWrapper.GetTeamname(HomeTeam) + neutral + " -vs- " + DataRefWrapper.GetTeamname(AwayTeam);
                return matchName;
            }
        }
    }
}