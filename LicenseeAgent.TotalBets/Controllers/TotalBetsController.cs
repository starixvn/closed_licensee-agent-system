// -----------------------------------------------------------------------
// <copyright file="TotalBetsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Account controllers.
    /// </summary>
    public class TotalBetsController : BaseController
    {
        private readonly ITotalBetsService _totalBetsService;

        public TotalBetsController(ITotalBetsService totalBetsService)
        {
            _totalBetsService = totalBetsService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}