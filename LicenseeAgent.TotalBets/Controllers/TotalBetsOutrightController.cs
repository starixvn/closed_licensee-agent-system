// -----------------------------------------------------------------------
// <copyright file="TotalBetsOutrightController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public partial class TotalBetsController : BaseController
    {
        [HttpGet]
        public ViewResult Outright()
        {
            var model = new SoccerViewModel() { League = string.Empty, Event = string.Empty, TimeInterval = 15 };

            return View(model);
        }

        [HttpPost]
        public JsonResult GetOutrightData(SoccerCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var dataList = _totalBetsService.GetSoccerData(criteria);
            return Json(new
            {
                List = MapTotalBetsMainData(dataList).OrderByDescending(x => x.LiveIndicator).ThenBy(x => x.LeagueName).ToList()
            });
        }

        #region Ultilities

        #endregion Ultilities
    }
}