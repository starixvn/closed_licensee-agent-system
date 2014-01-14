// ----------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/14">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class ReportController : BaseController
    {
        #region Constant

        #endregion

        #region Services

        #endregion

        #region Constructor

        #endregion

        #region Actions

        /// <summary>
        /// Index page of Results report.
        /// </summary>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Results()
        {
            return View();
        }

        #endregion

        #region Utilities
        #endregion
    }
}
