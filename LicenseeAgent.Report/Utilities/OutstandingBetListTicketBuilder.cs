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
            ShowOdds = false;
            IsShowMember = false;
            FormatDetail = "outstandingReport.showSystemParlayResult({0},{1},\"{2}\")";
            ClickFunction = "onclick=\"javascript: outstandingReport.showBetListDetails({0},'{1}')\"";
        }

        public override string BuildStatus(LasTicket ticket)
        {
            return BetListBuilder.BuildTotalBetsParlayStatus(ticket, TicketHelper, TicketResult.TicketDataList);
        }

        public override long BuildCustId(LasTicket ticket)
        {
            return ticket.CustId;
        }
    }
}