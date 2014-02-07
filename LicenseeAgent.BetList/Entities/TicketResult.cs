// ----------------------------------------------------------------------
// <copyright file="TicketResult.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Entities
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
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
