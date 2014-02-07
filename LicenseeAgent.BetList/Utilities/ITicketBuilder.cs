// ---------------------------------------------------------------------------------
// <copyright file="ITicketBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Paul.Nguyen" date="2014/02/06">Created.</change>
// </history>
// ---------------------------------------------------------------------------------

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