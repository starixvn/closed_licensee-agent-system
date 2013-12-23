// -----------------------------------------------------------------------
// <copyright file="SoccerDataEntity.cs" company="Nexcel Solutions Vietnam">
// SoccerDataEntity.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    /// <summary>
    /// Soccer Data Entity.
    /// </summary>
    public class SoccerDataEntity
    {
        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool IsLive { get; set; }

        public int BetType { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }

        public bool LiveIndicator { get; set; }

        public string BetTeam { get; set; }

        public int TotalTrans { get; set; }

        public decimal Stake { get; set; }
    }
}