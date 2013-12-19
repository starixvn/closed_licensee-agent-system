// ----------------------------------------------------------------------
// <copyright file="SearchUserController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
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
