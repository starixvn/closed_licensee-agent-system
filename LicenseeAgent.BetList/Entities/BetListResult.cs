namespace LicenseeAgent.BetList.Entities
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TicketResult class.
    /// </summary>
    public class BetListResult
    {
        public BetListResult()
        {
            MainTicketList = new List<LasTicket>();
            TicketDataList = new List<ITicketData>();
        }

        public List<LasTicket> MainTicketList { get; set; }

        public List<ITicketData> TicketDataList { get; set; }

        public int TotalRows { get; set; }
    }
}