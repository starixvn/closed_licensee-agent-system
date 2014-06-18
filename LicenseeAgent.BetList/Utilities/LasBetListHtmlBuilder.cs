// ----------------------------------------------------------------------
// <copyright file="LasBetListHtmlBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Utilities
{
    using System;
    using System.Collections.Generic;
    using Criteria;
    using Entities;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Builder.TransBuilder;
    using Fanex.BetList.Core.Entities;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// LasBetListHtmlBuilder class.
    /// </summary>
    public static class LasBetListHtmlBuilder
    {
        private const string _TRANS_BUILDER_NAMESPACE = "Fanex.BetList.Core.Builder.TransBuilder";

        /// <summary>
        /// Get ticket status.
        /// </summary>
        /// <param name="builder">Bet list builder.</param>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <param name="ticketHelper">Ticket helper.</param>
        /// <param name="ticketData">List of ticket data.</param>
        /// <param name="criteria">Meta model.</param>
        /// <param name="currencyID">Currency ID.</param>
        /// <param name="currencyName">Currency Name.</param>
        /// <returns>Ticket status text.</returns>
        public static string BuildLasBetListResultStatus(
            this BetListHTMLBuilder builder,
            LasTicket ticket,
            ITicketHelper ticketHelper,
            IList<ITicketData> ticketData,
            BetListCriteriaBase criteria,
            int currencyID,
            string currencyName)
        {
            const string StatusTemp = "<div class='status'>{0}</div>";
            var statusText = GetStatusText(ticket);
            var statusNormalText = string.Format(StatusTemp, statusText);

            switch (ticket.BetTypeId)
            {
                case (int)BetType.BetType_71:
                    string casinoHandTemplate = "<div id='casino-hand-history-{0}'";
                    casinoHandTemplate += "'result casino-hand-history-link' url-param='{1}' onclick='BetList.ViewCasinoHandHistory({0});'>{2}</div>";
                    return statusNormalText + string.Format(
                        casinoHandTemplate,
                        ticket.RefNo,
                        GetCasinoHandHistoryParam(ticket, criteria, currencyID, currencyName),
                        "Results");
                case (int)BetType.BetType_72:
                case (int)BetType.BetType_73:
                    return statusNormalText;

                default:
                    return builder.BuildStatus(ticket, ticketHelper, (List<ITicketData>)ticketData, true);
            }
        }

        /// <summary>
        /// Get ticket status.
        /// </summary>
        /// <param name="builder">Bet list builder.</param>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <param name="ticketHelper">Ticket helper.</param>
        /// <param name="ticketData">List of ticket data.</param>
        /// <returns>
        /// Ticket status text.
        /// </returns>
        public static string BuildTotalBetsParlayStatus(
            this BetListHTMLBuilder builder,
            LasTicket ticket,
            ITicketHelper ticketHelper,
            IList<ITicketData> ticketData)
        {
            const string StatusTemp = "<div class='status'>{0}</div>";
            var statusText = GetStatusText(ticket);
            var statusNormalText = string.Format(StatusTemp, statusText);

            switch (ticket.BetTypeId)
            {
                case (int)BetType.BetType_71:
                case (int)BetType.BetType_72:
                case (int)BetType.BetType_73:
                    return statusNormalText;

                default:
                    return builder.BuildStatus(ticket, ticketHelper, (List<ITicketData>)ticketData, true);
            }
        }

        /// <summary>
        /// Get ticket Agent PT.
        /// </summary>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <returns>Ticket Stake text.</returns>
        public static string BuildLasStake(Ticket ticket)
        {
            return "<div class=\"stake\">" + string.Format("{0:#,##0.00}", ticket.Stake) + "</div>";
        }

        /// <summary>
        /// Get ticket Win loss.
        /// </summary>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <returns>Ticket win loss text.</returns>
        public static string BuildLasWinloss(Ticket ticket)
        {
            if (ticket.Status == Constants.BETLISTTICKETSTATUSRUNNING || ticket.Status == Constants.BETLISTTICKETSTATUSWAITING)
            {
                return "-";
            }

            var lasWinlossHtmlString = ticket.Winlost >= 0
                ? string.Format("{0:#,##0.00}", ticket.Winlost) + "<br />"
                : "<div class=\"red-text\">" + string.Format("{0:#,##0.00}", ticket.Winlost) + "</div>";

            return lasWinlossHtmlString + string.Format("{0:#,##0.00}", ticket.PlayerComm);
        }

        /// <summary>
        /// Get ticket Agent PT.
        /// </summary>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <returns>Ticket agent position taking text.</returns>
        public static string BuildAgentPT(LasTicket ticket)
        {
            var colorValue = ticket.AgentPositionTaking >= 0 ? "\"stake\">" : "\"red-text\">";
            var agentPTHtmlStr = "<div class=\"stake\">" + string.Format("{0:#,##0.00}", ticket.AgentPTPercent * 100) + "%</div>";
            agentPTHtmlStr += (ticket.Status == Constants.BETLISTTICKETSTATUSRUNNING || ticket.Status == Constants.BETLISTTICKETSTATUSWAITING)
                ? string.Empty
                : "<div class=" + colorValue + Formatter.FormatDecimal(ticket.AgentPositionTaking) + "</div>";
            return agentPTHtmlStr;
        }

        /// <summary>
        /// Get ticket Agent Commission.
        /// </summary>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <returns>Ticket agent commission text.</returns>
        public static string BuildAgentComm(LasTicket ticket)
        {
            var agentCommHtmlStr = "<div class=\"stake\">" + string.Format("{0:#,##0.00}", ticket.AgentCommPercent * 100) + "%</div>";
            agentCommHtmlStr += (ticket.Status == Constants.BETLISTTICKETSTATUSRUNNING || ticket.Status == Constants.BETLISTTICKETSTATUSWAITING)
                ? string.Empty
                : "<div class=\"stake\">" + Formatter.FormatDecimal(ticket.AgentComm) + "</div>";
            return agentCommHtmlStr;
        }

        /// <summary>
        /// Get ticket Admin Agent Commission.
        /// </summary>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <returns>Ticket Admin agent commission text.</returns>
        public static string BuildAdminAgentComm(Ticket ticket)
        {
            if (ticket.Status == Constants.BETLISTTICKETSTATUSRUNNING || ticket.Status == Constants.BETLISTTICKETSTATUSWAITING)
            {
                return "-";
            }

            return "<div class=\"stake\">" + Formatter.FormatDecimal(ticket.AgentComm) + "</div>";
        }

        /// <summary>
        /// Builds the trans.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>The html string of Transaction Time.</returns>
        public static string BuildTrans(ITicket ticket)
        {
            ITrans transBuilder = CreateTransBuilder(ticket.BetTypeId);
            string trans = transBuilder.Render(ticket);
            return trans;
        }

        /// <summary>
        /// Creates the Transaction Time builder.
        /// </summary>
        /// <param name="bettype">The BETTYPE.</param>
        /// <returns>The html string of Transaction Time.</returns>
        private static ITrans CreateTransBuilder(int bettype)
        {
            var typeName = string.Join(null, new[] { _TRANS_BUILDER_NAMESPACE, ".Trans", bettype.ToString() });
            var type = Type.GetType(typeName);

            return type == null ? new LasTrans() : Activator.CreateInstance(type) as ITrans;
        }

        /// <summary>
        /// Get Casino Hand History Parameter.
        /// </summary>
        /// <param name="ticket">LRF Ticket object.</param>
        /// <param name="criteria">Meta model object.</param>
        /// <param name="currencyID">Currency ID.</param>
        /// <param name="currencyName">Currency Name.</param>
        /// <returns>Parameter for Casino Hand History.</returns>
        private static string GetCasinoHandHistoryParam(LasTicket ticket, BetListCriteriaBase criteria, int currencyID, string currencyName)
        {
            var param = string.Empty;
            param += "?RefNo=" + ticket.RefNo;
            param += "&FromDate=" + criteria.FromDate;
            param += "&ToDate=" + criteria.ToDate;
            param += "&SystemId=" + (string.IsNullOrEmpty(ticket.UserName) ? string.Empty : ticket.UserName);
            param += "&CustId=" + (ticket.CustId == 0 ? criteria.CustId : ticket.CustId);
            param += "&CurrencyId=" + currencyID.ToString();
            param += "&CurrencyName=" + currencyName;
            return param;
        }

        /// <summary>
        /// Get status text by status id.
        /// </summary>
        /// <param name="ticket">LRF ticket object.</param>
        /// <returns>Status text.</returns>
        private static string GetStatusText(LasTicket ticket)
        {
            var statusText = string.Empty;

            switch (ticket.Status.ToLower())
            {
                case BetStatus.Running:
                    statusText = CoreBetList.running;
                    break;

                case BetStatus.Reject:
                    statusText = CoreBetList.reject;
                    break;

                case BetStatus.Won:
                    statusText = CoreBetList.won;
                    break;

                case BetStatus.Lose:
                    statusText = CoreBetList.lose;
                    break;

                case BetStatus.Draw:
                    statusText = CoreBetList.draw;
                    break;

                case BetStatus.Waiting:
                    statusText = CoreBetList.waiting;
                    break;

                case BetStatus.Void:
                    statusText = CoreBetList.voided;
                    break;

                case BetStatus.Refund:
                    statusText = CoreBetList.refund;
                    break;
            }

            return statusText;
        }
    }
}