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
    public class ParlayController : BetListBase, IBetListBase
    {
        private readonly IBetListService _betlistService;
        private readonly IBelongingCheckingService _belongingCheckingService;

        public ParlayController(
            IBetListService betlistService,
            IBelongingCheckingService belongingCheckingService)
        {
            _betlistService = betlistService;
            _belongingCheckingService = belongingCheckingService;
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
            var ticketBuilderParameters = new TicketBuilderParameters()
            {
                TicketResult = ticketResults,
                IsAdmin = CurrentSession.IsAdmin
            };
            var betListTicketBuilder = new ParlayTicketBuilder(ticketBuilderParameters);

            var model = new BetListViewModel()
            {
                TicketTextList = betListTicketBuilder.BuildTickets(),
                TicketResult = ticketResults
            };

            return Json(new
            {
                List = model.TicketTextList,
                IsLastLevel = CurrentSession.AgentInfo.IsLastLevel,
                TotalItem = model.TicketTextList.Count
            });
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
            if (!_belongingCheckingService.IsDownLineTransaction(transactionBelongingChecking))
            {
                this.RedirectToAccessDeniedPage();
            }

            var agentId = CurrentSession.AgentInfo.AgentId;
            var criteria = new TotalBetsMixParlayDetailCriteria { AgentId = agentId, TransId = transId };
            var ticketResult = _betlistService.GetTotalBetsMixParlayDetail(criteria);

            return this.BuildMixParlayDetails(ticketResult);
        }

        /// <summary>
        /// System Parlay Details action.
        /// </summary>
        /// <param name="refNo">Reference number of transaction.</param>
        /// <returns>
        /// String content.
        /// </returns>
        [HttpGet]
        public ActionResult SystemParlayDetails(long refNo)
        {
            var agentId = CurrentSession.AgentInfo.AgentId;

            var criteria = new TotalBetsSystemParlayDetailCriteria
            {
                AgentId = agentId,
                RefNo = refNo
            };

            var systemParlayData = _betlistService.GetTotalBetsSystemParlayDetail(criteria);
            systemParlayData.Outstanding = criteria.Outstanding;
            var systemParlayText = this.BuildSystemParlayDetail(systemParlayData);

            return Content(systemParlayText);
        }

        /// <summary>
        /// Bet List Details.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult ParlayDetails(TotalBetsParlayDetailCriteria criteria)
        {
            var transactionBelongingChecking = new TransactionBelongingChecking()
            {
                TransactionId = criteria.TransId
            };
            if (!_belongingCheckingService.IsDownLineTransaction(transactionBelongingChecking))
            {
                this.RedirectToAccessDeniedPage();
            }

            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var rawBetListDetails = _betlistService.GetParlayDetail(criteria);
            var viewModel = new BetListDetailsViewModel();
            viewModel.BetListDetails = this.FormatRawBetListDetails(rawBetListDetails, criteria.Status);

            return View("~/Views/BetList/ParlayDetail.cshtml", viewModel);
        }
    }
}