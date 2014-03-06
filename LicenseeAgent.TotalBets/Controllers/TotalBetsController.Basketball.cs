// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.Basketball.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Utility;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public partial class TotalBetsController : BaseController
    {
        [HttpGet]
        public ActionResult Basketball()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BasketballAllBetType(string matchName)
        {
            ViewBag.Match = matchName;
            return View();
        }

        /// <summary>
        /// Get Basketball Data.
        /// </summary>
        /// <param name="model">Total Bets View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetBasketballData(TotalBetsViewModel model)
        {
            if (ModelState.IsValid)
            {
                BasketballCriteria criteria = new BasketballCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    LeagueList = model.LeagueList,
                    MatchList = model.MatchList
                };
                var dataList = _totalBetsService.GetBasketballData(criteria);
                var orderedDataList = TotalBetsUtility.MapTotalBetsMainData(dataList)
                    .OrderByDescending(x => x.LiveIndicator)
                    .ThenBy(x => x.LeagueName)
                    .ToList();

                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    List = orderedDataList
                });
            }

            return Json(new Result(Result.FailureValue));
        }

        /// <summary>
        /// Gets the basketball bet list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns JSON object.</returns>
        [HttpPost]
        public JsonResult GetBasketballBetList(BetListByMatchCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var tickets = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters()
            {
                TicketResult = tickets,
                IsAdmin = CurrentSession.IsAdmin
            };

            var betListTicketBuilder = new TotalbetsTicketBuilder(ticketBuilderParameters);
            
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = betListTicketBuilder.BuildTickets(),
                IsLastLevel = CurrentSession.AgentInfo.IsLastLevel,
                TotalItem = criteria.TotalRows
            });
        }
    }
}