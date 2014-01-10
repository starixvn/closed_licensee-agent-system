// ----------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
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
    public class BetListController : BaseController
    {
        #region Constant

        private const string BetListCustomerInfo = "BetListCustomerInfo";

        private const int CacheDuration = 43200;

        #endregion

        #region Services

        private IBetListService _betlistService;

        private ICoreService _coreService;

        #endregion

        #region Constructor

        public BetListController(IBetListService betlistService, ICoreService coreService)
        {
            _betlistService = betlistService;
            _coreService = coreService;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult BetListSelByMember(int custId)
        {
            ViewBag.CustId = custId;
            return View();
        }

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="criteria">Bet List By Member Criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult BetListSelByMember(BetListSelByMemberCriteria criteria)
        {
            var balanceUp = _coreService.GetBalanceUpDay();
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            criteria.MoveBetsDay = balanceUp.MoveBetDay;
            criteria.BalanceUpDay = balanceUp.BalanceUpDay;
            criteria.StatusId = criteria.StatusId == 255 ? null : criteria.StatusId;

            var ticketResults = _betlistService.GetBetList(criteria);

            var model = new BetListViewModel()
            {
                TicketTextList = Ticket(ticketResults, criteria),
                TicketResult = ticketResults
            };

            return Json(new
            {
                List = model.TicketTextList,
                TotalItem = criteria.TotalRows
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
            var criteria = new MixParlayDetailsCriteria { AgentId = agentId, TransId = transId };
            var ticketResult = _betlistService.GetMixParlayDetails(criteria);
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
        /// System Parlay Result action.
        /// </summary>
        /// <param name="refNo">Reference number of transaction.</param>
        /// <param name="custId">Customer ID.</param>
        /// <returns>String content.</returns>
        [HttpGet]
        [DownLineMember]
        public ActionResult SystemParlayResult(long refNo, int custId)
        {
            var agentId = CurrentSession.AgentInfo.AgentId;
            var balanceUp = _coreService.GetBalanceUpDay();

            var criteria = new SystemParlayResultCriteria
            {
                AgentId = agentId,
                RefNo = refNo,
                CustId = custId,
                BalanceUpDay = balanceUp.BalanceUpDay,
                MoveBetsDay = balanceUp.MoveBetDay
            };

            var systemParlayData = _betlistService.GetSystemParlayResult(criteria);
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
        public ActionResult Details(BetListDetailsCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var rawBetListDetails = _betlistService.GetBetListDetails(criteria);
            var viewModel = new BetListDetailsViewModel();
            viewModel.BetListDetails = FormatRawBetListDetails(rawBetListDetails);

            return View(viewModel);
        }

        #endregion

        #region Utilities - Bet List

        /// <summary>
        /// Generate ticket text.
        /// </summary>
        /// <param name="ticketResult">Ticket result Parameter value.</param>
        /// <param name="criteria"> Parameter value.</param>
        /// <param name="startIndex">Start index Parameter value.</param>
        /// <param name="showOdds">Show Odds or not.</param>
        /// <returns>Result value.</returns>
        [NonAction]
        private static List<TicketText> Ticket(
            TicketResult ticketResult,
            BetListCriteriaBase criteria,
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
                    var custId = ticket.CustId != 0 ? ticket.CustId : criteria.CustId;
                    var betTeam = ticketHelper.GetBetTypeNameById(ticket.BetTypeId) + " - " + ticketHelper.GetCombinationMPOfferName(ticket.BetTeam);
                    var formatDetail = "BETLIST.showSystemParlayResult({0},{1},\"{2}\")";
                    ticketHelper.ClientDetailSystemParlayFunction = string.Format(
                        formatDetail,
                        ticket.RefNo,
                        custId,
                        betTeam);
                }

                ticketHelper.IsShowOddsWithChoice = showOdds;
                var ticketText = new TicketText
                {
                    Index = startIndex++,
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = builder.BuildTrans(ticket),
                    Status = builder.BuildLasBetListResultStatus(
                        ticket, ticketHelper, ticketResult.TicketDataList, criteria, 20, "UUS"),
                    Choice = builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false),
                    Odds = builder.BuildOdds(ticket, ticketResult.TicketDataList, CacheRef.GetOddsTypeName),
                    Stake = builder.BuildStake(ticket),
                    AgentPT = builder.BuildAgentPT(ticket),
                    AgentComm = builder.BuildAgentComm(ticket),
                    WinLoss = ticket.Winlost >= 0
                        ? string.Format("{0:#,##0.00}", ticket.Winlost)
                        : "<div class=\"red-text\">" + string.Format("{0:#,##0.00}", ticket.Winlost) + "</div>"
                };

                var clickFunction = string.Format("onclick=\"javascript: BETLIST.showBetListDetails({0})\"", ticket.TransId);
                ticketText.Feature = string.Format(
                        "<a class=\"iplink\" href=\"javascript:;\" {0} >{1}</a>",
                        clickFunction,
                        Las.Details);

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }
        #endregion

        #region Utilities - Bet List Details

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
            var htmlString = "<div>" + string.Format("{0:#,##0}", percentage * 100);
            htmlString += "%</div><div" + colorValue + string.Format("{0:#,##0.00}", value) + "</div>";

            return htmlString;
        }

        #endregion
    }
}
