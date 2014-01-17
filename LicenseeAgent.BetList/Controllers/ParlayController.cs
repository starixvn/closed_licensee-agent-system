// ----------------------------------------------------------------------
// <copyright file="ParlayController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014/1/16">Created.</change>
// </history>

namespace LicenseeAgent.BetList.Controllers
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using Entities;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.TicketHelper;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Filters;
    using Services;
    using ViewModels;
    using Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ParlayController : BaseController
    {
        private IBetListService _betlistService;
        private ICoreService _coreService;

        public ParlayController(IBetListService betlistService, ICoreService coreService)
        {
            _betlistService = betlistService;
            _coreService = coreService;
        }

        [HttpGet]
        public ActionResult ParlayList()
        {
            return View("~/Views/BetList/ParlayList.cshtml");
        }

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="criteria">Bet List By Member Criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult ParlayList(TotalBetsParlayListCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var ticketResults = _betlistService.GetParlayList(criteria);

            var model = new BetListViewModel()
            {
                TicketTextList = ComposeTickets(ticketResults, criteria),
                TicketResult = ticketResults
            };

            return Json(new
            {
                List = model.TicketTextList,
                TotalItem = model.TicketTextList.Count
            });
        }

        /// <summary>
        /// Mix Parlay Details.
        /// </summary>
        /// <param name="transId">Transaction ID.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        [DownLineTransaction]
        public ActionResult MixParlayDetails(long transId)
        {
            var agentId = CurrentSession.AgentInfo.AgentId;
            var criteria = new TotalBetsMixParlayDetailCriteria { AgentId = agentId, TransId = transId };
            var ticketResult = _betlistService.GetTotalBetsMixParlayDetail(criteria);
            var builder = new BetListHTMLBuilder();
            var ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };

            var stringBuilder = new StringBuilder();
            foreach (var ticket in ticketResult.MainTicketList)
            {
                var choice = builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false);
                stringBuilder.AppendFormat("<div>{0}</div><div class='line'></div>", choice);
            }

            return Content(stringBuilder.ToString());
        }

        /// <summary>
        /// System Parlay Details action.
        /// </summary>
        /// <param name="refNo">Reference number of transaction.</param>
        /// <returns>
        /// String content.
        /// </returns>
        [HttpGet]
        [DownLineMember]
        public ActionResult SystemParlayDetails(long refNo)
        {
            var agentId = CurrentSession.AgentInfo.AgentId;

            var criteria = new TotalBetsSystemParlayDetailCriteria
            {
                AgentId = agentId,
                RefNo = refNo
            };

            var systemParlayData = _betlistService.GetTotalBetsSystemParlayDetail(criteria);
            var builder = new BetListHTMLBuilder();
            var ticketHelper = new TicketHelper();
            var systemParlayText = builder.BuildSystemParlayDetail(ticketHelper, systemParlayData);

            return Content(systemParlayText);
        }

        /// <summary>
        /// Bet List Details.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        [DownLineTransaction]
        public ActionResult ParlayDetails(TotalBetsParlayDetailCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var rawBetListDetails = _betlistService.GetParlayDetail(criteria);
            var viewModel = new BetListDetailsViewModel();
            viewModel.BetListDetails = FormatRawBetListDetails(rawBetListDetails);

            return View("~/Views/BetList/ParlayDetail.cshtml", viewModel);
        }

        /// <summary>
        /// Generate ticket text.
        /// </summary>
        /// <param name="ticketResult">Ticket result Parameter value.</param>
        /// <param name="criteria"> Parameter value.</param>
        /// <param name="startIndex">Start index Parameter value.</param>
        /// <param name="showOdds">Show Odds or not.</param>
        /// <returns>Result value.</returns>
        [NonAction]
        private static List<TicketText> ComposeTickets(
            TicketResult ticketResult,
            TotalBetsParlayListCriteria criteria,
            int startIndex = 1,
            bool showOdds = false)
        {
            var builder = new BetListHTMLBuilder();
            var ticketTextList = new List<TicketText>();
            var ticketHelper = new TicketHelper();

            foreach (var ticket in ticketResult.MainTicketList)
            {
                if (ticket.BetTypeId == (int)BetType.SystemParlay)
                {
                    var betTeam = ticketHelper.GetBetTypeNameById(ticket.BetTypeId) + " - " + ticketHelper.GetCombinationMPOfferName(ticket.BetTeam);
                    var formatDetail = "fanex.totalBets.parlay.showSystemParlayDetails({0},{1},\"{2}\")";
                    ticketHelper.ClientDetailSystemParlayFunction = string.Format(
                        formatDetail,
                        ticket.RefNo,
                        ticket.CustId,
                        betTeam);
                }

                ticketHelper.IsShowOddsWithChoice = showOdds;
                var ticketText = new TicketText
                {
                    Index = startIndex++,
                    Member = "victorm7",
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = builder.BuildTrans(ticket),
                    Status = builder.BuildTotalBetsParlayStatus(
                        ticket, ticketHelper, ticketResult.TicketDataList, criteria, 20, "UUS"),
                    Choice = builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false),
                    Odds = builder.BuildOdds(ticket, ticketResult.TicketDataList, CacheRef.GetOddsTypeName),
                    Stake = builder.BuildLasStake(ticket),
                    AgentPT = builder.BuildAgentPT(ticket),
                    AgentComm = builder.BuildAgentComm(ticket),
                    WinLoss = builder.BuildLasWinloss(ticket)
                };

                var clickFunction = string.Format("onclick=\"javascript: fanex.totalBets.parlay.showParlayDetails({0})\"", ticket.TransId);
                ticketText.Feature = string.Format(
                        "<a class=\"iplink\" href=\"javascript:;\" {0} >{1}</a>",
                        clickFunction,
                        Las.Details);

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        /// <summary>
        /// Format raw bet list details.
        /// </summary>
        /// <param name="originalData">List of raw bet list details.</param>
        /// <returns>List of formatted bet list details.</returns>
        private static List<BetListDetails> FormatRawBetListDetails(IEnumerable<RawBetListDetails> originalData)
        {
            var formattedData = new List<BetListDetails>();
            foreach (var item in originalData)
            {
                formattedData.Add(new BetListDetails
                {
                    Agent = item.AgentName,
                    PositionTaking = GenerateHtmlStringForAgent(item.AgentPTPercent, item.AgentPT),
                    Commission = GenerateHtmlStringForAgent(item.AgentCommPercent, item.AgentComm)
                });
            }

            return formattedData;
        }

        private static string GenerateHtmlStringForAgent(decimal percentage, decimal value)
        {
            var colorValue = value >= 0 ? ">" : " class=\"red-text\">";
            var htmlString = "<div>" + string.Format("{0:#,##0.00}", percentage * 100);
            htmlString += "%</div><div" + colorValue + string.Format("{0:#,##0.00}", value) + "</div>";

            return htmlString;
        }
    }
}
