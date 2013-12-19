// ----------------------------------------------------------------------
// <copyright file="TicketText.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketText
    {
        public int Index { get; set; }

        public string TransTime { get; set; }

        public string Choice { get; set; }

        public string Odds { get; set; }

        public string Stake { get; set; }

        public string WinLoss { get; set; }

        public string Status { get; set; }

        public string AgentPT { get; set; }

        public string AgentComm { get; set; }

        public string Feature { get; set; }
    }
}
