namespace LicenseeAgent.BetList.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Criteria;
    using Entities;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Builder.TransBuilder;
    using Fanex.BetList.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// LasBetListHtmlBuilder class.
    /// </summary>
    public static class LasBetListHtmlBuilder
    {
        /// <summary>
        /// Get ticket status.
        /// </summary>
        /// <param name="betListBuilder">Bet list builder.</param>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <param name="ticketHelper">Ticket helper.</param>
        /// <param name="ticketData">List of ticket data.</param>
        /// <param name="criteria">Meta model.</param>
        /// <param name="currencyId">Currency ID.</param>
        /// <param name="currencyName">Currency Name.</param>
        /// <returns>Ticket status text.</returns>
        public static string BuildBetListResultStatus(
            this BetListHTMLBuilder betListBuilder,
            LasTicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            BetListCriteriaBase criteria,
            int currencyId,
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
                        GetCasinoHandHistoryParam(ticket, criteria, currencyId, currencyName),
                        "Results");
                case (int)BetType.BetType_72:
                case (int)BetType.BetType_73:
                    return statusNormalText;

                default:
                    return betListBuilder.BuildStatus(ticket, ticketHelper, ticketData, true);
            }
        }

        /// <summary>
        /// Get ticket status.
        /// </summary>
        /// <param name="betListBuilder">Bet list builder.</param>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <param name="ticketHelper">Ticket helper.</param>
        /// <param name="ticketData">List of ticket data.</param>
        /// <returns>
        /// Ticket status text.
        /// </returns>
        public static string BuildTotalBetsParlayStatus(
            this BetListHTMLBuilder betListBuilder,
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
                    return betListBuilder.BuildStatus(ticket, ticketHelper, (List<ITicketData>)ticketData, true);
            }
        }

        /// <summary>
        /// Get ticket Agent PT.
        /// </summary>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <returns>Ticket Stake text.</returns>
        public static string BuildStake(Ticket ticket)
        {
            return "<div class=\"stake\">" + string.Format("{0:#,##0.00}", ticket.Stake) + "</div>";
        }

        /// <summary>
        /// Get ticket Win loss.
        /// </summary>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Ticket win loss text.
        /// </returns>
        public static string BuildWinloss(Ticket ticket, TicketBuilderParameters parameters)
        {
            if (ticket.Status == BetStatus.Running || ticket.Status == BetStatus.Waiting)
            {
                return "-";
            }

            var winLostText = ticket.Winlost >= 0
                ? string.Format("{0:#,##0.00}", ticket.Winlost) + "<br />"
                : "<div class=\"red-text\">" + string.Format("{0:#,##0.00}", ticket.Winlost) + "</div>";

            if (!parameters.IsViewCommission)
            {
                return winLostText;
            }

            var commText = string.Format("{0:#,##0.00}", ticket.PlayerComm);
            return string.Join(string.Empty, winLostText, commText);
        }

        /// <summary>
        /// Get ticket Agent PT.
        /// </summary>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>
        /// Ticket agent position taking text.
        /// </returns>
        public static string BuildAgentPT(LasTicket ticket, List<ITicketData> ticketData)
        {
            var stringBuilder = new StringBuilder();
            switch (ticket.BetTypeId)
            {
                case (int)BetType.BetType_1233:
                    var winTickets = ticketData.Where(x => x.BetTypeId == (int)BetType.BetType_1231 && x.RefNo == ticket.RefNo);
                    if (winTickets.Count() > 0)
                    {
                        var winTicket = (LasTicketData)winTickets.First();
                        stringBuilder.Append(string.Format(
                            "<span class='blue' style='font-weight:normal;'>{0}: <b>{1}</b></span><br />",
                            CoreBetList.win,
                            Formatter.FormatPercent(winTicket.AgentPTPercent)));
                    }

                    var placeTickets = ticketData.Where(x => x.BetTypeId == (int)BetType.BetType_1232 && x.RefNo == ticket.RefNo);
                    if (placeTickets.Count() > 0)
                    {
                        var placeTicket = (LasTicketData)placeTickets.First();
                        stringBuilder.Append(string.Format(
                            "<span style='font-weight:normal;'>{0}: <b>{1}</b></span>",
                            CoreBetList.place,
                            Formatter.FormatPercent(placeTicket.AgentPTPercent)));
                    }

                    break;

                default:
                    stringBuilder.Append("<div class=\"stake\">" + string.Format("{0:#,##0.00}", ticket.AgentPTPercent * 100) + "%</div>");
                    break;
            }

            if (ticket.Status != BetStatus.Running && ticket.Status != BetStatus.Waiting)
            {
                var colorValue = ticket.AgentPositionTaking >= 0 ? "\"stake\">" : "\"red-text\">";
                stringBuilder.Append("<div class=" + colorValue + Formatter.FormatDecimal(ticket.AgentPositionTaking) + "</div>");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get ticket Agent Commission.
        /// </summary>
        /// <param name="ticket">Affiliate ticket object.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isAdmin">If set to <c>true</c> [is admin].</param>
        /// <returns>
        /// Ticket agent commission text.
        /// </returns>
        public static string BuildAgentComm(LasTicket ticket, List<ITicketData> ticketData, bool isAdmin)
        {
            var stringBuilder = new StringBuilder();
            if (!isAdmin)
            {
                switch (ticket.BetTypeId)
                {
                    case (int)BetType.BetType_1233:
                        var winTickets = ticketData.Where(x => x.BetTypeId == (int)BetType.BetType_1231 && x.RefNo == ticket.RefNo);
                        if (winTickets.Count() > 0)
                        {
                            var winTicket = (LasTicketData)winTickets.First();
                            stringBuilder.Append(string.Format(
                                "<span class='blue' style='font-weight:normal;'>{0}: <b>{1}</b></span><br />",
                                CoreBetList.win,
                                Formatter.FormatPercent(winTicket.AgentCommPercent)));
                        }

                        var placeTickets = ticketData.Where(x => x.BetTypeId == (int)BetType.BetType_1232 && x.RefNo == ticket.RefNo);
                        if (placeTickets.Count() > 0)
                        {
                            var placeTicket = (LasTicketData)placeTickets.First();
                            stringBuilder.Append(string.Format(
                                "<span style='font-weight:normal;'>{0}: <b>{1}</b></span>",
                                CoreBetList.place,
                                Formatter.FormatPercent(placeTicket.AgentCommPercent)));
                        }

                        break;

                    default:
                        stringBuilder.Append("<div class=\"stake\">" + string.Format("{0:#,##0.00}", ticket.AgentCommPercent * 100) + "%</div>");
                        break;
                }
            }

            if (ticket.Status != BetStatus.Running && ticket.Status != BetStatus.Waiting)
            {
                stringBuilder.Append("<div class=\"stake\">" + Formatter.FormatDecimal(ticket.AgentComm) + "</div>");
            }

            return stringBuilder.ToString();
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
            var typeName = string.Format("Fanex.BetList.Core.Builder.TransBuilder.Trans{0}", bettype.ToString());
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

        private static string GetStatusText(LasTicket ticket)
        {
            return GetStatusText(ticket.Status);
        }

        /// <summary>
        /// Get status text by status id.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>
        /// Status text.
        /// </returns>
        private static string GetStatusText(string status)
        {
            switch (status.ToLower())
            {
                case BetStatus.Running:
                    return CoreBetList.running;

                case BetStatus.Reject:
                    return CoreBetList.reject;

                case BetStatus.Won:
                    return CoreBetList.won;

                case BetStatus.Lose:
                    return CoreBetList.lose;

                case BetStatus.Draw:
                    return CoreBetList.draw;

                case BetStatus.Waiting:
                    return CoreBetList.waiting;

                case BetStatus.Void:
                    return CoreBetList.voided;

                case BetStatus.Refund:
                    return CoreBetList.refund;
            }

            return string.Empty;
        }
    }
}