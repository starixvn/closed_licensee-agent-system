// ----------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Controllers
{
    using System.Web.Mvc;
    using Entities;
    using Fanex.Result.Core;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Services;
    using Services;
    using ViewModels;

    /// <summary>
    /// BetListController class.
    /// </summary>
    public class BetListController : BetListBase
    {
        private readonly IBetListService _betlistService;
        private readonly ICoreService _coreService;
        private readonly IDownlLinePermissionService _permissionService;

        public BetListController(
            IBetListService betlistService,
            ICoreService coreService,
            IDownlLinePermissionService permissionService)
            : base(permissionService)
        {
            _betlistService = betlistService;
            _coreService = coreService;
            _permissionService = permissionService;
        }

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
            criteria.MoveBetsDay = balanceUp.MoveBetsDay;
            criteria.BalanceUpDay = balanceUp.BalanceUpDay;
            criteria.StatusIDs = criteria.StatusIDs == "255" ? null : criteria.StatusIDs; // All

            var ticketResults = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = ticketResults,
                IsAdmin = CurrentSession.IsAdmin
            };
            var betListTicketBuilder = new BetListTicketBuilder(ticketBuilderParameters, criteria);

            var model = new BetListViewModel
            {
                TicketTextList = betListTicketBuilder.BuildTickets(),
                TicketResult = ticketResults
            };

            return Json(new
            {
                List = model.TicketTextList,
                IsLastLevel = CurrentSession.AgentInfo.IsLastLevel,
                TotalItem = criteria.TotalRows
            });
        }

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="model">Bet List Win loss View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult BetListWinlossReport(BetListWinLossViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Mix Parlay Details.
        /// </summary>
        /// <param name="transId">Transaction ID.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult MixParlayDetails(long transId)
        {
            if (!_permissionService.IsDownLineTransaction(transId))
            {
                return RedirectToAccessDeniedPage();
            }

            var agentId = CurrentSession.AgentInfo.AgentId;
            var criteria = new MixParlayDetailsCriteria { AgentId = agentId, TransId = transId };
            var ticketResult = _betlistService.GetMixParlayDetails(criteria);

            return BuildMixParlayDetails(ticketResult);
        }

        /// <summary>
        /// System Parlay Result action.
        /// </summary>
        /// <param name="refNo">Reference number of transaction.</param>
        /// <returns>String content.</returns>
        [HttpGet]
        public ActionResult SystemParlayResult(long refNo)
        {
            if (!_permissionService.IsDownLineReferenceNumber(refNo))
            {
                return RedirectToAccessDeniedPage();
            }

            var agentId = CurrentSession.AgentInfo.AgentId;
            var balanceUp = _coreService.GetBalanceUpDay();

            var criteria = new SystemParlayResultCriteria
            {
                AgentId = agentId,
                RefNo = refNo,
                BalanceUpDay = balanceUp.BalanceUpDay,
                MoveBetsDay = balanceUp.MoveBetsDay
            };

            var systemParlayData = _betlistService.GetSystemParlayResult(criteria);
            var systemParlayText = BuildSystemParlayDetail(systemParlayData);

            return Content(systemParlayText);
        }

        /// <summary>
        /// Bet List Details.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Details(BetListDetailsCriteria criteria)
        {
            if (!_permissionService.IsDownLineTransaction(criteria.TransId))
            {
                return RedirectToAccessDeniedPage();
            }

            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var rawBetListDetails = _betlistService.GetBetListDetails(criteria);
            var viewModel = new BetListDetailsViewModel
            {
                BetListDetails = FormatRawBetListDetails(rawBetListDetails, criteria.Status)
            };

            return View(viewModel);
        }

        /// <summary>
        /// Bet List Results.
        /// </summary>
        /// <param name="criteria">Bet list result criteria.</param>
        /// <returns>JSON result.</returns>
        public ActionResult Results(BetListResultCriteria criteria)
        {
            var setting = new ResultSetting
            {
                SportType = 0,
                IsOutRight = false,
                IsParlayPopUp = true,
                IsPopUp = true,
                LanguageCode = CurrentSession.Language
            };

            ViewBag.Data = criteria.IsSportsBook
                ? GetSportsBookResults(criteria.MatchId, setting)
                : GetParlayResults(criteria.RefNo, setting);

            return View();
        }

        /// <summary>
        /// Get Sports book Results.
        /// </summary>
        /// <param name="matchId">Input Match ID.</param>
        /// <param name="setting">Results Setting.</param>
        /// <returns>JSON object.</returns>
        public string GetSportsBookResults(int matchId, ResultSetting setting)
        {
            var data = _betlistService.GetSportsBookResults(new BetListResultsSportsBookCriteria { MatchId = matchId });
            return data != null
                ? ResultEngine.HtmlParser(data, setting)
                : string.Empty;
        }

        /// <summary>
        /// Get Parlay Results.
        /// </summary>
        /// <param name="refNo">Reference Number.</param>
        /// <param name="setting">Results Setting.</param>
        /// <returns>JSON object.</returns>
        public string GetParlayResults(long refNo, ResultSetting setting)
        {
            var data = _betlistService.GetParlayResults(new BetListResultsParlayCriteria { RefNo = refNo });
            return data != null
                ? ResultEngine.HtmlParser(data, setting)
                : string.Empty;
        }
    }
}