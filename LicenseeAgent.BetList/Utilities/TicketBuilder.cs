namespace LicenseeAgent.BetList.Utilities
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public abstract class TicketBuilder : ITicketBuilder
    {
        private const string Feature = "<a class=\"iplink\" href=\"javascript:;\" {0} >{1}</a>";
        private readonly IList<TicketText> _ticketTextList = new List<TicketText>();
        private readonly BetListHTMLBuilder _betListBuilder;
        private readonly ITicketHelper _ticketHelper;
        private int _startIndex = 1;

        protected TicketBuilder(TicketBuilderParameters ticketBuilderParameters)
            : this(null, null)
        {
            Parameters = ticketBuilderParameters;
        }

        private TicketBuilder(BetListHTMLBuilder betListBuilder, ITicketHelper ticketHelper)
        {
            ShowOdds = false;
            IsShowMember = false;
            ClickFunction = "onclick=\"javascript: fanex.betlist.core.showDetails({0}, '{1}', {2}, {3})\"";
            _betListBuilder = betListBuilder ?? new BetListHTMLBuilder();
            _ticketHelper = ticketHelper ?? new TicketHelper();
        }

        protected bool ShowOdds { get; set; }

        protected bool IsShowMember { get; set; }

        protected string ClickFunction { get; set; }

        protected TicketBuilderParameters Parameters { get; private set; }

        protected BetListResult TicketResult
        {
            get { return Parameters.TicketResult; }
        }

        protected BetListHTMLBuilder BetListBuilder
        {
            get { return _betListBuilder; }
        }

        protected ITicketHelper TicketHelper
        {
            get { return _ticketHelper; }
        }

        public abstract string BuildStatus(LasTicket ticket);

        public abstract long BuildCustId(LasTicket ticket);

        /// <summary>
        /// Generate ticket text.
        /// </summary>
        /// <returns>Result value.</returns>
        public IList<TicketText> BuildTickets()
        {
            foreach (var ticket in TicketResult.MainTicketList)
            {
                BuildOneTicket(ticket);
            }

            return _ticketTextList;
        }

        private void BuildOneTicket(LasTicket ticket)
        {
            InitializeTicketHelperInTheCaseOfSystemParlay(ticket);
            var ticketText = BuildTicketText(ticket);
            _ticketTextList.Add(ticketText);
        }

        /// <summary>
        /// Builds the ticket text.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The object of TicketText.</returns>
        private TicketText BuildTicketText(LasTicket ticket)
        {
            var ticketText = new TicketText
            {
                Index = _startIndex++,
                ExternalId = ticket.ExternalId,
                SystemId = ticket.UserName,
                TransTime = LasBetListHtmlBuilder.BuildTrans(ticket),
                Status = BuildStatus(ticket),
                Choice = _betListBuilder.BuildChoice(ticket, _ticketHelper, TicketResult.TicketDataList, false),
                Odds = _betListBuilder.BuildOdds(ticket, TicketResult.TicketDataList, CacheRef.GetOddsTypeName),
                Stake = LasBetListHtmlBuilder.BuildStake(ticket),
                AgentPT = LasBetListHtmlBuilder.BuildAgentPT(ticket, TicketResult.TicketDataList),
                AgentComm = LasBetListHtmlBuilder.BuildAgentComm(ticket, TicketResult.TicketDataList, Parameters.IsAdmin),
                WinLoss = LasBetListHtmlBuilder.BuildWinloss(ticket, Parameters)
            };

            if (IsShowMember)
            {
                ticketText.Member = ticket.UserName2;
            }

            ticketText.Feature = string.Format(
                    Feature,
                    BuildTheClickFunction(ticket),
                    Las.Details);
            return ticketText;
        }

        private string BuildTheClickFunction(LasTicket ticket)
        {
            var clickFunction = string.Format(
                ClickFunction,
                ticket.TransId,
                ticket.Status,
                ticket.SportTypeId,
                ticket.BetTypeId);
            return clickFunction;
        }

        /// <summary>
        /// Initializes the ticket helper in the case of system parlay.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        private void InitializeTicketHelperInTheCaseOfSystemParlay(LasTicket ticket)
        {
            if (ticket.BetTypeId == (int)BetType.BetType_29)
            {
                var betTeam = _ticketHelper.GetBetTypeNameById(ticket.BetTypeId) + " - " + _ticketHelper.GetCombinationMPOfferName(ticket.BetTeam);
                _ticketHelper.ClientDetailSystemParlayFunction = string.Format(
                    "fanex.betlist.core.showSystemParlayDetails({0},{1},\"{2}\")",
                    ticket.RefNo,
                    ticket.TransId,
                    betTeam);
            }

            _ticketHelper.IsShowOddsWithChoice = ShowOdds;
        }
    }
}