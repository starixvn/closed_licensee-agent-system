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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Entities;
    using Criteria;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Report.Entities;
    using LicenseeAgent.Taiwan.Actions;
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
            model.AgentId = CurrentSession.AgentInfo.AgentId;
            model.AgentLevel = CurrentSession.AgentInfo.AgentLevel;
            model.FromDate = model.FromDate.GetHashCode() == 0 ? DateTime.Now.AddDays(-1) : model.FromDate;
            model.ToDate = model.ToDate.GetHashCode() == 0 ? DateTime.Now : model.ToDate;

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
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    Username = model.Username
                }).ToList();
                var serviceResult = _memberService.GetMemberInformations<Statement>(
                                    result,
                                    new GetMemberInfoListAction
                                    {
                                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                                        LASAgentId = model.AgentId
                                    });

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    return Json(new
                    {
                        ErrCode = serviceResult.ErrCode,
                        ErrMsg = serviceResult.ErrMsg
                    });
                }
            }
            else
            {
                result = _reportService.StatementReport(new StatementAgentCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
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

        /// <summary>
        /// Statement Detail Report.
        /// </summary>
        /// <param name="model">Statement Report Detail View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult StatementDetail(StatementDetailReportViewModel model)
        {
            if (model.FromDate.GetHashCode() == 0 || model.ToDate.GetHashCode() == 0)
            {
                model.FromDate = DateTime.Now.AddDays(-1);
                model.ToDate = DateTime.Now;
            }

            ViewBag.HideUsername = false;
            if (Request["parentUsername"] != null)
            {
                ViewBag.ParentUsername = Request["parentUsername"];
            }

            ViewBag.PageContext = new PageContext(model.AgentId, model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel)
            {
                IsDirectDownline = CurrentSession.IsDirectDownline(model.AgentLevel),
                IsAdmin = CurrentSession.IsAdmin
            };
            return View(model);
        }

        /// <summary>
        /// Statement Detail Report.
        /// </summary>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult StatementDetailCurrentUser()
        {
            StatementDetailReportViewModel model = new StatementDetailReportViewModel
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                AgentLevel = CurrentSession.AgentInfo.AgentLevel,
                FromDate = DateTime.Now.AddDays(-1),
                ToDate = DateTime.Now,
                Username = CurrentSession.AgentInfo.UserName
            };
            ViewBag.HideUsername = true;

            ViewBag.PageContext = new PageContext(model.AgentId, model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel)
            {
                IsDirectDownline = CurrentSession.IsDirectDownline(model.AgentLevel),
                IsAdmin = CurrentSession.IsAdmin
            };
            return View("StatementDetail", model);
        }

        /// <summary>
        /// Statement Report Data.
        /// </summary>
        /// <param name="model">Statement Report Detail View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetStatementDetailData(StatementDetailReportViewModel model)
        {
            var balanceUp = _coreService.GetBalanceUpDay();
            var result = new List<Statement>();
            if (model.IsMember)
            {
                result = _reportService.StatementReport(new StatementMemberDetailCriteria
                {
                    UserId = CurrentSession.AgentInfo.AgentId,
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    CustUsername = model.Username
                }).ToList();
            }
            else
            {
                result = _reportService.StatementReport(new StatementAgentDetailCriteria
                {
                    UserId = CurrentSession.AgentInfo.AgentId,
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    AgentUsername = model.Username
                }).ToList();
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = result
            });
        }

        /// <summary>
        /// Statement Detail Report.
        /// </summary>
        /// <param name="model">Statement Report Detail View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult TransferDetail(TransferDetailViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Get Transfer Data.
        /// </summary>
        /// <param name="model">Transfer Detail View Model.</param>
        /// <returns>Return JSON.</returns>
        [HttpPost]
        public JsonResult GetTransferData(TransferDetailViewModel model)
        {
            var balanceUp = _coreService.GetBalanceUpDay();
            var result = new List<Transfer>();
            if (model.IsMember)
            {
                result = _reportService.TransferDetail(new TransferDetailMemberCriteria
                {
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    CustId = model.AgentId
                }).ToList();
            }
            else
            {
                result = _reportService.TransferDetail(new TransferDetailAgentCriteria
                {
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    AgentId = model.AgentId
                }).ToList();
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = result,
                Total = result.Sum(m => m.TransferAmt)
            });
        }
    }
}