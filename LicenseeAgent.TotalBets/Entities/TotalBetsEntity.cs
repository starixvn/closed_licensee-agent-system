// --------------------------------------------------------------------------
// <copyright file="TotalBetsEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    /// <summary>
    /// Total Bets Entity.
    /// </summary>
    public class TotalBetsEntity
    {
        public int SportType { get; set; }

        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool IsNeutral { get; set; }

        public int BetType { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }

        public bool LiveIndicator { get; set; }

        public string BetTeam { get; set; }

        public string ShowTime { get; set; }

        public int TotalTrans { get; set; }

        public decimal Stake { get; set; }
    }
}
