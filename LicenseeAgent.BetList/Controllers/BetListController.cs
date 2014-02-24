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
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using Entities;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Result.Core;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Services;
    using Services;
    using ViewModels;
    using Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListController : BetListBase, IBetListBase
    {
        private const string BetListCustomerInfo = "BetListCustomerInfo";
        private const int CacheDuration = 43200;
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
            criteria.StatusIDs = criteria.StatusIDs == "All" ? null : criteria.StatusIDs;

            var ticketResults = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters()
            {
                TicketResult = ticketResults,
                IsAdmin = CurrentSession.IsAdmin
            };
            var betListTicketBuilder = new BetListTicketBuilder(ticketBuilderParameters, criteria);

            var model = new BetListViewModel()
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
        public ActionResult BetListWinlossReport(BetListWinlossViewModel model)
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

            return this.BuildMixParlayDetails(ticketResult);
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
            var systemParlayText = this.BuildSystemParlayDetail(systemParlayData);

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
            var viewModel = new BetListDetailsViewModel();
            viewModel.BetListDetails = this.FormatRawBetListDetails(rawBetListDetails, criteria.Status);

            return View(viewModel);
        }

        /// <summary>
        /// Bet List Results.
        /// </summary>
        /// <param name="isSportsbook">Is sports book.</param>
        /// <param name="matchId">Input Match ID.</param>
        /// <param name="refNo">Reference Number.</param>
        /// <param name="sportType">Sport Type.</param>
        /// <returns>JSON Object.</returns>
        public ActionResult Results(bool isSportsbook, int matchId = 0, long refNo = 0, int sportType = 0)
        {
            var setting = new ResultSetting
            {
                SportType = 0,
                IsOutRight = false,
                IsParlayPopUp = true,
                IsPopUp = true,
                LanguageCode = CurrentSession.Language
            };

            ViewBag.Data = isSportsbook
                ? GetSportsbookResults(matchId, setting)
                : GetParlayResults(refNo, setting);

            return View();
        }

        /// <summary>
        /// Get Sports book Results.
        /// </summary>
        /// <param name="matchId">Input Match ID.</param>
        /// <param name="setting">Results Setting.</param>
        /// <returns>JSON object.</returns>
        public string GetSportsbookResults(int matchId, ResultSetting setting)
        {
            var data = _betlistService.GetSportsbookResults(new BetListResultsSportsbookCriteria { MatchId = matchId });
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