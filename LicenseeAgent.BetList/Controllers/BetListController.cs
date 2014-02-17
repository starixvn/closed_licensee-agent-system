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
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Core.Configurations;
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
        private readonly IPermissionService _permissionService;

        public BetListController(
            IBetListService betlistService,
            ICoreService coreService,
            IPermissionService permissionService)
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
            var transactionBelongingChecking = new TransactionBelongingChecking()
            {
                TransactionId = transId
            };
            if (!_permissionService.IsDownlineTransaction(transactionBelongingChecking))
            {
                this.RedirectToAccessDeniedPage();
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
        /// <param name="custId">Customer ID.</param>
        /// <returns>String content.</returns>
        [HttpGet]
        public ActionResult SystemParlayResult(long refNo, int custId)
        {
            var belongingCheckingBase = new CustomerBelongingCheckingBase()
            {
                TargetCustId = custId
            };
            var referenceNumberBelongingChecking = new ReferenceNumberBelongingChecking()
            {
                ReferenceNumber = refNo
            };

            if (!_permissionService.IsDownlineMember(belongingCheckingBase)
                || !_permissionService.IsDownlineReferenceNumber(referenceNumberBelongingChecking))
            {
                this.RedirectToAccessDeniedPage();
            }

            var agentId = CurrentSession.AgentInfo.AgentId;
            var balanceUp = _coreService.GetBalanceUpDay();

            var criteria = new SystemParlayResultCriteria
            {
                AgentId = agentId,
                RefNo = refNo,
                CustId = custId,
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
            var transactionBelongingChecking = new TransactionBelongingChecking()
            {
                TransactionId = criteria.TransId
            };
            if (!_permissionService.IsDownlineTransaction(transactionBelongingChecking))
            {
                this.RedirectToAccessDeniedPage();
            }

            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var rawBetListDetails = _betlistService.GetBetListDetails(criteria);
            var viewModel = new BetListDetailsViewModel();
            viewModel.BetListDetails = this.FormatRawBetListDetails(rawBetListDetails, criteria.Status);

            return View(viewModel);
        }
    }
}
