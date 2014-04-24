// ---------------------------------------------------------------------------------
// <copyright file="TicketBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Utilities
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        private readonly bool _isAdmin;
        private readonly Collection<TicketText> _ticketTextList;
        private int _startIndex = 1;
        private bool _showOdds;
        private bool _isShowMember;
        private string _formatDetail = "fanex.betlist.core.showSystemParlayDetails({0},{1},\"{2}\")";
        private string _clickFunction = "onclick=\"javascript: fanex.betlist.core.showDetails({0}, '{1}', {2}, {3})\"";
        private TicketResult _ticketResult;
        private BetListHTMLBuilder _builder;
        private TicketHelper _ticketHelper;
        
        protected TicketBuilder(TicketBuilderParameters ticketBuilderParameters)
        {
            _ticketResult = ticketBuilderParameters.TicketResult;
            _isAdmin = ticketBuilderParameters.IsAdmin;
            _builder = new BetListHTMLBuilder();
            _ticketTextList = new Collection<TicketText>();
            _ticketHelper = new TicketHelper();
        }

        protected bool ShowOdds
        {
            get { return _showOdds; }
            set { _showOdds = value; }
        }

        protected int StartIndex
        {
            get { return _startIndex; }
            set { _startIndex = value; }
        }

        protected bool IsShowMember
        {
            get { return _isShowMember; }
            set { _isShowMember = value; }
        }

        protected string FormatDetail
        {
            get { return _formatDetail; }
            set { _formatDetail = value; }
        }

        protected string ClickFunction
        {
            get { return _clickFunction; }
            set { _clickFunction = value; }
        }

        protected TicketResult TicketResult
        {
            get { return _ticketResult; }
            set { _ticketResult = value; }
        }

        protected BetListHTMLBuilder Builder
        {
            get { return _builder; }
            set { _builder = value; }
        }

        protected TicketHelper TicketHelper
        {
            get { return _ticketHelper; }
            set { _ticketHelper = value; }
        }

        public abstract void Initialize();

        public abstract string BuildStatus(LasTicket ticket);

        public abstract long BuildCustId(LasTicket ticket);

        /// <summary>
        /// Generate ticket text.
        /// </summary>
        /// <returns>Result value.</returns>
        public Collection<TicketText> BuildTickets()
        {
            Initialize();

            foreach (var ticket in _ticketResult.MainTicketList)
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
                Choice = _builder.BuildChoice(ticket, _ticketHelper, (List<ITicketData>)_ticketResult.TicketDataList, false),
                Odds = _builder.BuildOdds(ticket, (List<ITicketData>)_ticketResult.TicketDataList, CacheRef.GetOddsTypeName),
                Stake = LasBetListHtmlBuilder.BuildLasStake(ticket),
                AgentPT = LasBetListHtmlBuilder.BuildAgentPT(ticket),
                AgentComm = _isAdmin ? LasBetListHtmlBuilder.BuildAdminAgentComm(ticket) : LasBetListHtmlBuilder.BuildAgentComm(ticket),
                WinLoss = LasBetListHtmlBuilder.BuildLasWinloss(ticket)
            };

            if (_isShowMember)
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
                _clickFunction,
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
                    _formatDetail,
                    ticket.RefNo,
                    BuildCustId(ticket),
                    betTeam);
            }

            _ticketHelper.IsShowOddsWithChoice = _showOdds;
        }
    }
}