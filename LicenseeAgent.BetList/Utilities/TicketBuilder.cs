// ---------------------------------------------------------------------------------
// <copyright file="TicketBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Paul.Nguyen" date="2014/02/06">Created.</change>
// </history>
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
        private const string _Feature = "<a class=\"iplink\" href=\"javascript:;\" {0} >{1}</a>";
        private int _startIndex = 1;
        private bool _showOdds = false;
        private bool _isShowMember = false;
        private string _formatDetail = "fanex.betlist.core.showSystemParlayDetails({0},{1},\"{2}\")";
        private string _clickFunction = "onclick=\"javascript: fanex.betlist.core.showDetails({0}, '{1}')\"";
        private TicketResult _ticketResult;
        private BetListHTMLBuilder _builder;
        private TicketHelper _ticketHelper;
        private bool _isAdmin = false;
        private Collection<TicketText> _ticketTextList;

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
            this.Initialize();

            foreach (var ticket in this._ticketResult.MainTicketList)
            {
                this.BuildOneTicket(ticket);
            }

            return _ticketTextList;
        }

        private void BuildOneTicket(LasTicket ticket)
        {
            this.InitializeTicketHelperInTheCaseOfSystemParlay(ticket);
            var ticketText = this.BuildTicketText(ticket);
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
                Index = this._startIndex++,
                ExternalId = ticket.ExternalId,
                SystemId = ticket.UserName,
                TransTime = LasBetListHtmlBuilder.BuildTrans(ticket),
                Status = this.BuildStatus(ticket),
                Choice = _builder.BuildChoice(ticket, _ticketHelper, (List<ITicketData>)this._ticketResult.TicketDataList, false),
                Odds = _builder.BuildOdds(ticket, (List<ITicketData>)this._ticketResult.TicketDataList, CacheRef.GetOddsTypeName),
                Stake = _builder.BuildLasStake(ticket),
                AgentPT = _builder.BuildAgentPT(ticket),
                AgentComm = this._isAdmin ? _builder.BuildAdminAgentComm(ticket) : _builder.BuildAgentComm(ticket),
                WinLoss = _builder.BuildLasWinloss(ticket)
            };

            if (this._isShowMember)
            {
                ticketText.Member = ticket.UserName2;
            }

            ticketText.Feature = string.Format(
                    _Feature,
                    this.BuildTheClickFunction(ticket),
                    Las.Details);
            return ticketText;
        }

        private string BuildTheClickFunction(LasTicket ticket)
        {
            var clickFunction = string.Format(
                this._clickFunction,
                ticket.TransId,
                ticket.Status);
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
                    this._formatDetail,
                    ticket.RefNo,
                    this.BuildCustId(ticket),
                    betTeam);
            }

            _ticketHelper.IsShowOddsWithChoice = this._showOdds;
        }
    }
}