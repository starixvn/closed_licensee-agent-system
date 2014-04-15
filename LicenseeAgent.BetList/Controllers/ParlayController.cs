// ----------------------------------------------------------------------
// <copyright file="ParlayController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Controllers
{
    using System.Web.Mvc;
    using Entities;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Web.Services;
    using Services;
    using ViewModels;

    /// <summary>
    /// ParlayController class.
    /// </summary>
    public class ParlayController : BetListBase
    {
        private readonly IBetListService _betlistService;

        public ParlayController(
            IBetListService betlistService,
            IDownlLinePermissionService permissionService)
            : base(permissionService)
        {
            _betlistService = betlistService;
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
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = ticketResults,
                IsAdmin = CurrentSession.IsAdmin
            };
            var betListTicketBuilder = new TotalbetsTicketBuilder(ticketBuilderParameters);

            var model = new BetListViewModel
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
    }
}