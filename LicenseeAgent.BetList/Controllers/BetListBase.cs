// ----------------------------------------------------------------------
// <copyright file="BetListBase.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014/2/6">Created</change>
// </history>

namespace LicenseeAgent.BetList.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BetListBase : BaseController, IBetListBase
    {
        protected BetListBase(IPermissionService permissionService)
            : base(permissionService)
        {
        }

        /// <summary>
        /// Format raw bet list details.
        /// </summary>
        /// <param name="originalData">List of raw bet list details.</param>
        /// <param name="status">Ticket status ID.</param>
        /// <returns>List of formatted bet list details.</returns>
        public Collection<BetListDetails> FormatRawBetListDetails(IEnumerable<RawBetListDetails> originalData, string status)
        {
            var formattedData = new Collection<BetListDetails>();
            foreach (var item in originalData)
            {
                formattedData.Add(new BetListDetails
                {
                    Agent = item.AgentName,
                    PositionTaking = GenerateHtmlStringForAgentPT(item, status),
                    Commission = GenerateHtmlStringForAgentComm(item, status)
                });
            }

            return formattedData;
        }

        public string BuildSystemParlayDetail(SystemParlayData systemParlayData)
        {
            var builder = new BetListHTMLBuilder();
            var ticketHelper = new TicketHelper() { ShowSystemParlayPlayerComm = true };
            var systemParlayText = builder.BuildSystemParlayDetail(ticketHelper, systemParlayData);
            return systemParlayText;
        }

        /// <summary>
        /// Builds the mix parlay details.
        /// </summary>
        /// <param name="ticketResult">The ticket result.</param>
        /// <returns>The object of ActionResult.</returns>
        public ActionResult BuildMixParlayDetails(TicketResult ticketResult)
        {
            var builder = new BetListHTMLBuilder();
            var ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };

            var stringBuilder = new StringBuilder();
            foreach (var ticket in ticketResult.MainTicketList)
            {
                var choice = builder.BuildChoice(ticket, ticketHelper, (List<ITicketData>)ticketResult.TicketDataList, false);
                stringBuilder.AppendFormat("<div class='ticketList'>{0}</div><div class='line'></div>", choice);
            }

            return Content(stringBuilder.ToString());
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.CheckValidCurrency();
            base.OnActionExecuting(filterContext);
        }

        private static string GenerateHtmlStringForAgentPT(RawBetListDetails data, string status)
        {
            var colorValue = data.AgentPT >= 0 ? ">" : " class=\"red-text\">";
            var htmlString = "<div>" + string.Format("{0:#,##0.00}", data.AgentPTPercent * 100) + "%</div>";
            htmlString += (status == Constants.BETLISTTICKETSTATUSRUNNING || status == Constants.BETLISTTICKETSTATUSWAITING)
                ? string.Empty
                : "<div" + colorValue + string.Format("{0:#,##0.00}", data.AgentPT) + "</div>";

            return htmlString;
        }

        private static string GenerateHtmlStringForAgentComm(RawBetListDetails data, string status)
        {
            var htmlString = data.AgentLevel == 1
                ? string.Empty
                : "<div>" + string.Format("{0:#,##0.00}", data.AgentCommPercent * 100) + "%</div>";
            htmlString += (status == Constants.BETLISTTICKETSTATUSRUNNING || status == Constants.BETLISTTICKETSTATUSWAITING)
                ? (data.AgentLevel == 1 ? "-" : string.Empty)
                : "<div>" + string.Format("{0:#,##0.00}", data.AgentComm) + "</div>";

            return htmlString;
        }
    }
}
