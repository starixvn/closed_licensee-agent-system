namespace LicenseeAgent.Report.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Entities;
    using Criteria;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Report.Entities;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Entities;
    using ViewModels;

    /// <summary>
    /// Report Controller.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Verified")]
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
            model.AgentId = model.AgentId > 0 ? model.AgentId : CurrentSession.AgentInfo.AgentId;
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedPage();
            }

            model.AgentLevel = model.AgentLevel > 0 ? model.AgentLevel : CurrentSession.AgentInfo.AgentLevel;
            model.UserName = !string.IsNullOrEmpty(model.UserName) ? model.UserName : CurrentSession.AgentInfo.UserName;
            model.BreadCrumb = !string.IsNullOrEmpty(model.BreadCrumb)
                                ? model.BreadCrumb
                                : model.AgentId + "," + model.AgentLevel + "," + model.UserName;
            model.ProductList = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            model.IsDeposit = CurrentSession.AgentInfo.IsDeposit;
            ViewBag.PageContext = new PageContext(CurrentSession, model.AgentId, model.AgentLevel);
            return View(model);
        }

        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <param name="model">Agent Win Loss Report View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Fix later.")]
        public JsonResult GetAgentWinLossData(AgentWinLossReportViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var balUp = _coreService.BalanceUpDay;
            CriteriaBase criteria;
            if (ModelHelper.IsLastAgent(model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel))
            {
                criteria = new WinLossMemberCriteria(
                    model.AgentId, model.FromDate, model.ToDate, balUp.MoveBetsDay, balUp.BalanceUpDay, (int)model.DrawFilter, model.Product);
            }
            else if (ModelHelper.IsAgentLevel0(model.AgentLevel))
            {
                criteria = new WinLossSuperViewAgentCriteria(
                    model.AgentId, model.FromDate, model.ToDate, balUp.MoveBetsDay, balUp.BalanceUpDay, (int)model.DrawFilter, model.Product);
            }
            else
            {
                criteria = new WinLossAgentViewAgentCriteria(
                    model.AgentId, model.FromDate, model.ToDate, balUp.MoveBetsDay, balUp.BalanceUpDay, (int)model.DrawFilter, model.Product);
            }

            var result = _reportService.AgentWinLossReport(criteria).ToList();
            var totalRow = from x in result
                           group x by 1 into g
                           select new AgentWinLoss
                           {
                               Username = Las.Total,
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