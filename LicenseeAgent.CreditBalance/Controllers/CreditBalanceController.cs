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
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;

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
        /// Agents the specified agent identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns agent CreditBalance page.
        /// </returns>
        [HttpGet]
        public ViewResult Agent(CreditBalanceAgentViewModel model)
        {
            if (model.Amount > 0)
            {
                model.MinAmount = 0;
                model.MaxAmount = model.Amount;
            }
            else
            {
                model.MinAmount = model.Amount;
                model.MaxAmount = 0;
            }

            return View(model);
        }

        [HttpPost, ActionName("Agent")]
        public ActionResult AgentPost(CreditBalanceAgentViewModel model)
        {
            return Json(new
            {
                ErrCode = Result.SuccessValue
            });
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