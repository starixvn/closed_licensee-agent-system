// ----------------------------------------------------------------------
// <copyright file="ReportController.AgentWinloss.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

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
        /// <param name="model">Agent Win Loss Report View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult AgentWinLoss(AgentWinLossReportViewModel model)
        {
            var isAdmin = CurrentSession.IsAdmin;
            model.AgentId = model.AgentId > 0 ? model.AgentId : CurrentSession.AgentInfo.AgentId;
            if (!_permissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedPage();
            }

            model.AgentLevel = model.AgentLevel > 0 ? model.AgentLevel : CurrentSession.AgentInfo.AgentLevel;
            model.Username = !string.IsNullOrEmpty(model.Username)
                ? model.Username
                : (isAdmin ? CurrentSession.AgentInfo.LicenseeName : CurrentSession.AgentInfo.UserName);
            model.BreadCrumb = !string.IsNullOrEmpty(model.BreadCrumb)
                                ? model.BreadCrumb
                                : model.AgentId + "," + model.AgentLevel + "," + model.Username;
            model.ProductList = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);

            ViewBag.PageContext = new PageContext(model.AgentId, model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel)
            {
                IsDirectDownline = CurrentSession.IsDirectDownline(model.AgentLevel),
                IsAdmin = CurrentSession.IsAdmin
            };
            return View(model);
        }

        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <param name="model">Agent Win Loss Report View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetAgentWinLossData(AgentWinLossReportViewModel model)
        {
            if (!_permissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var balUp = _coreService.GetBalanceUpDay();
            List<AgentWinLoss> result;
            if (ModelHelper.IsLastAgent(model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel))
            {
                var criteria = new WinLossMemberCriteria(
                                    model.AgentId, model.FromDate, model.ToDate, balUp.MoveBetsDay, balUp.BalanceUpDay, model.DrawFilter, model.Product);
                result = _reportService.AgentWinLossReport(criteria).ToList();
            }
            else if (ModelHelper.IsAgentLevel0(model.AgentLevel))
            {
                var criteria = new WinLossSuperViewAgentCriteria(
                        model.AgentId, model.FromDate, model.ToDate, balUp.MoveBetsDay, balUp.BalanceUpDay, model.DrawFilter, model.Product);
                result = _reportService.AgentWinLossReport(criteria).ToList();
            }
            else
            {
                var criteria = new WinLossAgentViewAgentCriteria(
                        model.AgentId, model.FromDate, model.ToDate, balUp.MoveBetsDay, balUp.BalanceUpDay, model.DrawFilter, model.Product);
                result = _reportService.AgentWinLossReport(criteria).ToList();
            }

            var totalRow = from x in result 
                           group x by 1 into g 
                           select new
                           {
                               BetCount = g.Sum(x => x.BetCount),
                               Turnover = g.Sum(x => x.Turnover),
                               Winlost = g.Sum(x => x.Winlost),
                               PlayerComm = g.Sum(x => x.PlayerComm),
                               MemberTotal = g.Sum(x => x.MemberTotal),
                               DownlineStake = g.Sum(x => x.DownlineStake),
                               DownlineTotal = g.Sum(x => x.DownlineTotal),
                               AWinLost = g.Sum(x => x.AWinLost),
                               AComm = g.Sum(x => x.AComm),
                               ATotal = g.Sum(x => x.ATotal),
                               UplineStake = g.Sum(x => x.UplineStake),
                               UplineTotal = g.Sum(x => x.UplineTotal)
                           };
            return Json(new { ErrCode = Result.SuccessValue, Total = totalRow, List = result });
        }
    }
}