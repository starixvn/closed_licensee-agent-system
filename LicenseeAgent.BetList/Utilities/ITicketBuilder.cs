namespace LicenseeAgent.BetList.Utilities
{
    using System.Collections.Generic;
    using LicenseeAgent.BetList.Entities;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public interface ITicketBuilder
    {
        IList<TicketText> BuildTickets();

        string BuildStatus(LasTicket ticket);

        long BuildCustId(LasTicket ticket);
    }
}