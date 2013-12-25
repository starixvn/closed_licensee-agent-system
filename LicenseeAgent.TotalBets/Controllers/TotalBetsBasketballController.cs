// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Resources;
    using LicenseeAgent.TotalBets.Criterias;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.TotalBets.Entities;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public partial class TotalBetsController : BaseController
    {
        [HttpGet]
        public ActionResult Basketball()
        {
            var model = new TotalBetsViewModel() { League = string.Empty, Event = string.Empty, TimeInterval = 15 };

            return GetBasketballData(model);
        }

        [HttpPost]
        public ActionResult Basketball(TotalBetsViewModel model)
        {
            return GetBasketballData(model);
        }

        [HttpGet]
        public ActionResult BasketballAllBetType(int matchId, string matchName)
        {
            ViewBag.Match = matchName;
            return View();
        }

        [NonAction]
        public ActionResult GetBasketballData(TotalBetsViewModel model)
        {
            ////var criteria = new BasketballCriteria { AgentId = CurrentSession.AgentInfo.AgentId, LeagueList = model.League, MatchList = model.Event };

            var criteria = new BasketballCriteria { AgentId = 1000000111, LeagueList = string.Empty, MatchList = string.Empty };
            var dataList = _totalBetsService.GetBasketballData(criteria);
            model.MainData = GetMainData(dataList);

            return View(model);
        }
    }
}