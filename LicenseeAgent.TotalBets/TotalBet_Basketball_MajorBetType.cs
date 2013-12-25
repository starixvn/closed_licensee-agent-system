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

    public class TotalBet_Basketball_MajorBetType
    {
        public int? matchid { get; set; }

        public short? bettype { get; set; }

        public short? LiveHomeScore { get; set; }

        public short? LiveAwayScore { get; set; }

        public bool? LiveIndicator { get; set; }

        public string BetTeam { get; set; }

        public int? TotalTrans { get; set; }

        public decimal? Stake { get; set; }

        public int? leagueid { get; set; }

        public int? homeid { get; set; }

        public int? awayid { get; set; }

        public DateTime? kickofftime { get; set; }

        public bool? IsLive { get; set; }
    }
}
