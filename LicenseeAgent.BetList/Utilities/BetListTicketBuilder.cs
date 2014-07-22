namespace LicenseeAgent.BetList.Utilities
{
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Entities;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public class BetListTicketBuilder : TicketBuilder
    {
        private readonly BetListCriteriaBase _criteria;

        public BetListTicketBuilder(TicketBuilderParameters ticketBuilderParameters, BetListCriteriaBase criteria)
            : base(ticketBuilderParameters)
        {
            _criteria = criteria;
            ShowOdds = false;
            IsShowMember = false;
            ClickFunction = "onclick=\"javascript: fanex.betlist.core.showDetails({0},'{1}',{2},{3})\"";
        }

        public override string BuildStatus(LasTicket ticket)
        {
            return BetListBuilder.BuildBetListResultStatus(ticket, TicketHelper, TicketResult.TicketDataList, _criteria, 20, "UUS");
        }

        public override long BuildCustId(LasTicket ticket)
        {
            return _criteria.CustId != 0 ? ticket.CustId : _criteria.CustId;
        }
    }
}