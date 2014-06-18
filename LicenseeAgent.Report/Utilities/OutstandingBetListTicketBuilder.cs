// ----------------------------------------------------------------------
// <copyright file="OutstandingBetListTicketBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Report.Utilities
{
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Utilities;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public class OutstandingBetListTicketBuilder : TicketBuilder, ITicketBuilder
    {
        public OutstandingBetListTicketBuilder(TicketBuilderParameters ticketBuilderParameters)
            : base(ticketBuilderParameters)
        {
        }

        public override void Initialize()
        {
            ShowOdds = false;
            StartIndex = 1;
            IsShowMember = false;
            FormatDetail = "outstandingReport.showSystemParlayResult({0},{1},\"{2}\")";
            ClickFunction = "onclick=\"javascript: outstandingReport.showBetListDetails({0},'{1}')\"";
        }

        public override string BuildStatus(LasTicket ticket)
        {
            return Builder.BuildTotalBetsParlayStatus(ticket, TicketHelper, TicketResult.TicketDataList);
        }

        public override long BuildCustId(LasTicket ticket)
        {
            return ticket.CustId;
        }
    }
}