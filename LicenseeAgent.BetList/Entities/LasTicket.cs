// ----------------------------------------------------------------------
// <copyright file="LasTicket.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Entities
{
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LasTicket : Ticket
    {
        public decimal AgentPT { get; set; }

        public decimal AgentPTAmount { get; set; }

        public decimal AgentCommAmount { get; set; }
    }
}
