// --------------------------------------------------------------------------
// <copyright file="TotalBet_Basketball_MajorBetType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.12.25">Created</change>
// </history>

namespace LicenseeAgent.TotalBets.Entities
{
    using System;

    /// <summary>
    /// Total Bets Basketball Major Bet Type.
    /// </summary>
    public class TotalBet_Basketball_MajorBetType
    {
        public int? MatchId { get; set; }

        public short? BetType { get; set; }

        public short? LiveHomeScore { get; set; }

        public short? LiveAwayScore { get; set; }

        public bool? LiveIndicator { get; set; }

        public string BetTeam { get; set; }

        public int? TotalTrans { get; set; }

        public decimal? Stake { get; set; }

        public int? LeagueId { get; set; }

        public int? HomeId { get; set; }

        public int? AwayId { get; set; }

        public DateTime? KickOffTime { get; set; }

        public bool? IsLive { get; set; }
    }
}
