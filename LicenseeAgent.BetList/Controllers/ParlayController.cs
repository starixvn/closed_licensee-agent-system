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
        private readonly IPermissionService _permissionService;

        public ParlayController(
            IBetListService betlistService,
            IPermissionService permissionService)
            : base(permissionService)
        {
            _betlistService = betlistService;
            _permissionService = permissionService;
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
    }
}