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

    public class LasTicket : Ticket
    {
        public string ExternalId { get; set; }

        public decimal AgentPTPercent { get; set; }

        public decimal AgentCommPercent { get; set; }

        public string UserName2 { get; set; }
    }
}
