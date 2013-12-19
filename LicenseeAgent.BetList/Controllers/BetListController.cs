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
    using System.Web.Mvc;
    using Services;
    using Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListController : BaseController
    {
        private readonly IBetListService _betlistService;

        public BetListController(IBetListService betlistService)
        {
            _betlistService = betlistService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
