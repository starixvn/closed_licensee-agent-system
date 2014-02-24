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
        /// Statement Report.
        /// </summary>
        /// <param name="model">Statement Report View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Statement(StatementReportViewModel model)
        {
            model.AgentId = model.AgentId > 0 ? model.AgentId : CurrentSession.AgentInfo.AgentId;
            model.AgentLevel = model.AgentLevel > 0 ? model.AgentLevel : CurrentSession.AgentInfo.AgentLevel;
            
            model.Username = !string.IsNullOrEmpty(model.Username) ? model.Username : CurrentSession.AgentInfo.LicenseeName;

            ViewBag.PageContext = new PageContext(model.AgentId, model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel)
            {
                IsDirectDownline = CurrentSession.IsDirectDownline(model.AgentLevel),
                IsAdmin = CurrentSession.IsAdmin
            };
            return View(model);
        }

        /// <summary>
        /// Statement Report Data.
        /// </summary>
        /// <param name="model">Statement Report View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetStatementData(StatementReportViewModel model)
        {
            var balanceUp = _coreService.GetBalanceUpDay();
            var result = new List<Statement>();
            if (ModelHelper.IsLastAgent(model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel))
            {
                result = _reportService.StatementReport(new StatementMemberCriteria
                {
                    AgentId = model.AgentId,
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    MoveBetsDay = balanceUp.MoveBetsDay,
                    ToDate = model.ToDate,
                    Username = model.Username
                }).ToList();
            }
            else
            {
                result = _reportService.StatementReport(new StatementAgentCriteria
                {
                    AgentId = model.AgentId,
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    MoveBetsDay = balanceUp.MoveBetsDay,
                    ToDate = model.ToDate,
                    Username = model.Username
                }).ToList();
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = result
            });
        }
    }
}