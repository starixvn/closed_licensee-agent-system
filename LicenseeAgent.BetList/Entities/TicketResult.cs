// ----------------------------------------------------------------------
// <copyright file="TicketResult.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Entities
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TicketResult class.
    /// </summary>
    public class TicketResult
    {
        public TicketResult()
        {
            MainTicketList = new List<LasTicket>();
            TicketDataList = new List<ITicketData>();
        }

        public IList<LasTicket> MainTicketList { get; set; }

        public IList<ITicketData> TicketDataList { get; set; }

        public int TotalRows { get; set; }
    }
}
