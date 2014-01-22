// --------------------------------------------------------------------------
// <copyright file="CreditBalanceController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.Controllers
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.CreditBalance.Criteria;
    using LicenseeAgent.CreditBalance.Services;
    using LicenseeAgent.CreditBalance.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CreditBalanceController : BaseController
    {
        private readonly ICreditBalanceService _creditBalanceService;

        public CreditBalanceController(ICreditBalanceService creditBalanceService)
        {
            _creditBalanceService = creditBalanceService;
        }

        public ActionResult Index()
        {
            return View("~/Views/Account/CreditBalance/Index.cshtml");
        }

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns list of customer balance.</returns>
        [HttpPost]
        public JsonResult Search(CreditBalanceSearchViewModel model)
        {
            if (SessionManager.CurrentSession.AgentInfo.IsLastLevel)
            {
                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    List = _creditBalanceService.GetMemberBalanceInfo(new GetMemberBalanceInfoCriteria
                    {
                        AgentId = CurrentSession.AgentInfo.AgentId,
                        Username = model.UserName
                    })
                });
            }
            else
            {
                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    List = _creditBalanceService.GetAgentBalanceInfo(new GetAgentBalanceInfoCriteria
                    {
                        AgentId = CurrentSession.AgentInfo.AgentId,
                        Username = model.UserName,
                        Status = model.Status
                    })
                });
            }
        }

        /// <summary>
        /// Licensees the edit agent credit.
        /// </summary>
        /// <param name="licenseeEditAgentCreditViewModel">The licensee edit agent credit view model.</param>
        /// <returns>The LicenseeEditAgentCredit view.</returns>
        [HttpGet]
        public ViewResult LicenseeEditAgentCredit(LicenseeEditAgentCreditViewModel licenseeEditAgentCreditViewModel)
        {
            var licenseeGetAgentRangeInfoCriteria = new LicenseeGetAgentRangeInfoCriteria
            {
                AgentId = licenseeEditAgentCreditViewModel.AgentId
            };

            _creditBalanceService.LicenseeGetAgentRangeInfo(licenseeGetAgentRangeInfoCriteria);
            licenseeEditAgentCreditViewModel.MinCreditOutput = licenseeGetAgentRangeInfoCriteria.MinCreditOutput;
            licenseeEditAgentCreditViewModel.MaxCreditOutput = licenseeGetAgentRangeInfoCriteria.MaxCreditOutput;

            return View("~/Views/Account/CreditBalance/LicenseeEditAgentCredit.cshtml", licenseeEditAgentCreditViewModel);
        }

        /// <summary>
        /// Licensees the edit agent credit post.
        /// </summary>
        /// <param name="licenseeEditAgentCreditViewModel">The licensee edit agent credit view model.</param>
        /// <returns>The LicenseeEditAgentCredit view.</returns>
        [HttpPost, ActionName("LicenseeEditAgentCredit")]
        public ActionResult LicenseeEditAgentCreditPost(LicenseeEditAgentCreditViewModel licenseeEditAgentCreditViewModel)
        {
            if (ModelState.IsValid)
            {
                var criteria = new LicenseeUpdateAgentCreditCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    Credit = licenseeEditAgentCreditViewModel.GivenCredit,
                    Ip = Request.GetClientIp(),
                    Updater = CurrentSession.LoginUserId
                };
                _creditBalanceService.LicenseeUpdateAgentCredit(criteria);

                if (criteria.MsgId != Result.SuccessValue)
                {
                    ModelState.AddModelError(criteria.MsgId.ToString(), GetErrMsg(criteria.MsgId));
                }
                else
                {
                    ViewBag.Result = Las.EditCreditSuccessfully;
                }
            }

            return View("~/Views/Account/CreditBalance/LicenseeEditAgentCredit.cshtml", licenseeEditAgentCreditViewModel);
        }

        /// <summary>
        /// Members the specified member identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Returns member CreditBalance page.</returns>
        [HttpGet]
        public ViewResult Member(int memberId)
        {
            var model = new CreditBalanceMemberViewModel
            {
                MemberId = memberId,
                BetCredit = 12345.678m,
                LastLogin = DateTime.Now,
                LastTransaction = DateTime.Now,
                Amount = 10,
                MinAmount = 0,
                MaxAmount = 100
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult Member(CreditBalanceMemberViewModel model)
        {
            return Json(new
            {
                ErrCode = Result.SuccessValue
            });
        }
    }
}