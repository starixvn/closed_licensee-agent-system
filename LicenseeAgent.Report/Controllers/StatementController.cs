namespace LicenseeAgent.Report.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Entities;
    using Criteria;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Report.Entities;
    using LicenseeAgent.Report.Services;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Services;
    using ViewModels;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class StatementController : BaseController
    {
        private readonly IDownLinePermissionService _downLinePermissionService;

        private readonly IMemberService _memberService;

        private readonly ICoreService _coreService;

        private readonly IStatementService _statementService;

        public StatementController()
            : this(null, null, null, null)
        {
        }

        public StatementController(
            ICoreService coreService,
            IStatementService statementService,
            IDownLinePermissionService downLinePermissionService,
            IMemberService memberService)
        {
            _coreService = coreService ?? new CoreService();
            _statementService = statementService ?? new StatementService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _memberService = memberService ?? new MemberService();
        }

        public string StatementProducts
        {
            get
            {
                var listProducts = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
                var productIds = listProducts.Where(x => x.ProductId != (int)ProductCode.Product_9).Select(x => x.ProductId).ToList();
                return string.Join(",", productIds);
            }
        }

        /// <summary>
        /// Statement Report.
        /// </summary>
        /// <param name="model">Statement Report View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Index(StatementReportViewModel model)
        {
            model.AgentId = CurrentSession.AgentInfo.AgentId;
            model.AgentLevel = CurrentSession.AgentInfo.AgentLevel;
            model.FromDate = model.FromDate.GetHashCode() == 0 ? DateTime.Now.AddDays(-1) : model.FromDate;
            model.ToDate = model.ToDate.GetHashCode() == 0 ? DateTime.Now : model.ToDate;
            ViewBag.PageContext = new PageContext(CurrentSession, model.AgentId, model.AgentLevel);
            return View(model);
        }

        /// <summary>
        /// Statement Report Data.
        /// </summary>
        /// <param name="model">Statement Report View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        [ActionName("Index")]
        public JsonResult IndexPost(StatementReportViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineAgent(CurrentSession.AgentInfo.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var balanceUp = _coreService.BalanceUpDay;
            IList<Statement> result;
            if (ModelHelper.IsLastAgent(model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel))
            {
                result = _statementService.GetStatementList(new StatementMemberCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    Username = model.UserName
                });
                var serviceResult = _memberService.GetMemberInformations(
                    result,
                    new GetMemberInfoListAction
                    {
                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                        LASAgentId = model.AgentId
                    });
                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    return JsonResult(serviceResult);
                }
            }
            else
            {
                result = _statementService.GetStatementList(new StatementAgentCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    Username = model.UserName
                });
            }

            return Json(new { ErrCode = Result.SuccessValue, List = result });
        }

        /// <summary>
        /// Statement Detail Report.
        /// </summary>
        /// <param name="model">Statement Report Detail View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Detail(StatementDetailReportViewModel model)
        {
            if (model.IsMember)
            {
                if (!_downLinePermissionService.IsDownLineMember(model.AgentId))
                {
                    return RedirectToAccessDeniedPage();
                }
            }
            else if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedPage();
            }

            if (model.FromDate.GetHashCode() == 0 || model.ToDate.GetHashCode() == 0)
            {
                model.FromDate = DateTime.Now.AddDays(-1);
                model.ToDate = DateTime.Now;
            }
            else
            {
                // From Downline statement page.
                model.ShowBackButton = true;
            }

            model.Product = StatementProducts;
            model.HideUsername = false;
            ViewBag.PageContext = new PageContext(CurrentSession, model.AgentId, model.AgentLevel);
            return View(model);
        }

        /// <summary>
        /// Statement Detail Report.
        /// </summary>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Current()
        {
            var model = new StatementDetailReportViewModel
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                AgentLevel = CurrentSession.AgentInfo.AgentLevel,
                FromDate = DateTime.Now.AddDays(-1),
                ToDate = DateTime.Now,
                Username = CurrentSession.AgentInfo.UserName,
                Product = StatementProducts,
                HideUsername = true
            };

            ViewBag.PageContext = new PageContext(CurrentSession, model.AgentId, model.AgentLevel);
            return View("Detail", model);
        }

        [HttpPost]
        public JsonResult Current(StatementDetailReportViewModel model)
        {
            return DetailPost(model);
        }

        /// <summary>
        /// Statement Report Data.
        /// </summary>
        /// <param name="model">Statement Report Detail View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        [ActionName("Detail")]
        public JsonResult DetailPost(StatementDetailReportViewModel model)
        {
            var balanceUp = _coreService.BalanceUpDay;
            var criteria = new StatementDetailCriteria
            {
                UserId = CurrentSession.AgentInfo.AgentId,
                BalanceUpDay = balanceUp.BalanceUpDay,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                Username = model.Username
            };

            var result = _statementService.GetStatementList(criteria);
            if (CurrentSession.AgentInfo.LegendType == LegendType.Deposit)
            {
                if (criteria.IsAgent)
                {
                    result = new List<Statement>();
                }

                criteria.IsAgent = false;
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = result,
                IsAgent = criteria.IsAgent
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
            if (model.IsMember)
            {
                if (!_downLinePermissionService.IsDownLineMember(model.CustId))
                {
                    return RedirectToAccessDeniedPage();
                }
            }
            else if (!_downLinePermissionService.IsDownLineAgent(model.CustId))
            {
                return RedirectToAccessDeniedPage();
            }

            return View(model);
        }

        /// <summary>
        /// Get Transfer Data.
        /// </summary>
        /// <param name="model">Transfer Detail View Model.</param>
        /// <returns>Return JSON.</returns>
        [HttpPost]
        [ActionName("TransferDetail")]
        public JsonResult TransferDetailPost(TransferDetailViewModel model)
        {
            var balanceUp = _coreService.BalanceUpDay;
            CriteriaBase criteria;
            if (model.IsMember)
            {
                criteria = new TransferDetailMemberCriteria
                {
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    CustId = model.CustId
                };
            }
            else
            {
                criteria = new TransferDetailAgentCriteria
                {
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    AgentId = model.CustId
                };
            }

            var result = _statementService.GetTransferDetailList(criteria);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = result,
                Total = result.Sum(m => m.TransferAmt)
            });
        }
    }
}