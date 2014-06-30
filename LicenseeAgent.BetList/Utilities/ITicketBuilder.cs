namespace LicenseeAgent.BetList.Utilities
{
    using System.Collections.ObjectModel;
    using LicenseeAgent.BetList.Entities;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public interface ITicketBuilder
    {
        Collection<TicketText> BuildTickets();

        void Initialize();

        string BuildStatus(LasTicket ticket);

        long BuildCustId(LasTicket ticket);
    }
}