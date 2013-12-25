// ----------------------------------------------------------------------
// <copyright file="SearchUserController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchUserController : BaseController
    {
        private readonly ISearchUserService _searchUserService;

        public SearchUserController(ISearchUserService searchUserService)
        {
            _searchUserService = searchUserService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}