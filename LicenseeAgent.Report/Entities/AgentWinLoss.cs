// --------------------------------------------------------------------------
// <copyright file="AgentWinLoss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.2.7">Created</change>
// </history>

namespace LicenseeAgent.Report.Entities
{
    using System;

    public class AgentWinLoss
    {
        public string Username { get; set; }

        public int AgentId { get; set; }

        public long BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winlost { get; set; }

        public decimal PlayerComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal AWinLost { get; set; }

        public decimal AComm { get; set; }

        public decimal ATotal { get; set; }

        public decimal DownlineStake { get; set; }

        public decimal DownlineTotal { get; set; }

        public decimal UplineEffStake { get; set; }

        public decimal UplineTotal { get; set; }
    }
}
