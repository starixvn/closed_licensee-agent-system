namespace LicenseeAgent.BetList.Utilities
{
    using LicenseeAgent.BetList.Entities;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public class TotalBetsTicketBuilder : TicketBuilder
    {
        public TotalBetsTicketBuilder(TicketBuilderParameters ticketBuilderParameters)
            : base(ticketBuilderParameters)
        {
            ShowOdds = false;
            IsShowMember = true;
            ClickFunction = "onclick=\"javascript: BET_LIST.showDetails({0}, '{1}',{2},{3})\"";
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