// ----------------------------------------------------------------------
// <copyright file="LasBetListHtmlBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Utilities
{
    using System.Collections.Generic;
    using Criteria;
    using Entities;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LasBetListHtmlBuilder
    {
        private const string BETLISTTICKETSTATUSRUNNING = "running";

        private const string BETLISTTICKETSTATUSWAITING = "waiting";

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
            List<ITicketData> ticketData,
            BetListCriteriaBase criteria,
            int currencyID,
            string currencyName)
        {
            const string StatusTemp = "<div class='status'>{0}</div>";
            var statusText = GetStatusText(ticket);
            var statusNormalText = string.Format(StatusTemp, statusText);

            switch (ticket.BetTypeId)
            {
                case 71:
                    string casinoHandTemplate = "<div id='casino-hand-history-{0}'";
                    casinoHandTemplate += "'result casino-hand-history-link' url-param='{1}' onclick='BetList.ViewCasinoHandHistory({0});'>{2}</div>";
                    return statusNormalText + string.Format(
                        casinoHandTemplate,
                        ticket.RefNo,
                        GetCasinoHandHistoryParam(ticket, criteria, currencyID, currencyName),
                        "Results");
                case 72:
                case 73:
                    return statusNormalText;

                default:
                    return builder.BuildStatus(ticket, ticketHelper, ticketData, true);
            }
        }

        /// <summary>
        /// Get ticket Agent PT.
        /// </summary>
        /// <param name="builder">Bet list builder.</param>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <returns>Ticket Stake text.</returns>
        public static string BuildLasStake(
            this BetListHTMLBuilder builder,
            LasTicket ticket)
        {
            return "<div class=\"stake\">" + string.Format("{0:#,##0.00}", ticket.Stake) + "</div>";
        }

        /// <summary>
        /// Get ticket Win loss.
        /// </summary>
        /// <param name="builder">Bet list builder.</param>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <returns>Ticket win loss text.</returns>
        public static string BuildLasWinloss(
            this BetListHTMLBuilder builder,
            LasTicket ticket)
        {
            if (ticket.Status == BETLISTTICKETSTATUSRUNNING || ticket.Status == BETLISTTICKETSTATUSWAITING)
            {
                return "-";
            }

            return ticket.Winlost > 0
                ? string.Format("{0:#,##0.00}", ticket.Winlost)
                : "<div class=\"red-text\">" + string.Format("{0:#,##0.00}", ticket.Winlost) + "</div>";
        }

        /// <summary>
        /// Get ticket Agent PT.
        /// </summary>
        /// <param name="builder">Bet list builder.</param>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <returns>Ticket status text.</returns>
        public static string BuildAgentPT(
            this BetListHTMLBuilder builder,
            LasTicket ticket)
        {
            var colorValue = ticket.AgentPositionTaking >= 0 ? "\"stake\">" : "\"red-text\">";
            var agentPTHtmlStr = "<div class=\"stake\">" + string.Format("{0:#,##0.00}", ticket.AgentPTPercent * 100);
            agentPTHtmlStr += "%</div><div class=" + colorValue + Formatter.FormatDecimal(ticket.AgentPositionTaking) + "</div>";
            return agentPTHtmlStr;
        }

        /// <summary>
        /// Get ticket Agent Commission.
        /// </summary>
        /// <param name="builder">Bet list builder.</param>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <returns>Ticket status text.</returns>
        public static string BuildAgentComm(
            this BetListHTMLBuilder builder,
            LasTicket ticket)
        {
            var agentCommHtmlStr = "<div class=\"stake\">" + string.Format("{0:#,##0.00}", ticket.AgentCommPercent * 100);
            agentCommHtmlStr += "%</div><div class=\"stake\">" + Formatter.FormatDecimal(ticket.AgentComm) + "</div>";
            return agentCommHtmlStr;
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
            param += "&BaseCurrency=" + criteria.BaseCurrency;

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