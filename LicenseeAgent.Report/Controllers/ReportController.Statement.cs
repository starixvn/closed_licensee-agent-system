// ----------------------------------------------------------------------
// <copyright file="ReportController.Statement.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/02/20">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Entities;
    using Criteria;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Report.Entities;
    using LicenseeAgent.Web;
    using ViewModels;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Statement()
        {
            return View();
        }

        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetStatementData()
        {
            return Json(new
            {
                ErrCode = Result.SuccessValue
            });
        }
    }
}