// --------------------------------------------------------------------------
// <copyright file="TransferController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Controllers
{
    using System;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Transfer.Criteria;
    using LicenseeAgent.Transfer.Services;
    using LicenseeAgent.Transfer.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferController : BaseController
    {
        private readonly ITransferService _transferService;

        public TransferController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns list of customer balance.</returns>
        [HttpPost]
        public JsonResult Search(TransferSearchViewModel model)
        {
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = _transferService.GetAgentBalanceInfo(new GetAgentBalanceInfoCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    Username = model.UserName,
                    Status = model.Status
                })
            });
        }

        /// <summary>
        /// Agents the specified agent identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns agent transfer page.
        /// </returns>
        [HttpGet]
        public ViewResult Agent(TransferAgentViewModel model)
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

        /// <summary>
        /// Agents the post.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns View.</returns>
        [HttpPost, ActionName("Agent")]
        [ValidateAntiForgeryToken]
        public ActionResult AgentPost(TransferAgentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new TransferToAgentCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    ToAgentId = model.AgentId,
                    Amount = model.Amount
                };
                _transferService.TransferToAgent(criteria);
                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ViewBag.Result = Las.TransferSuccessfully;
                }
            }

            return View(model);
        }

        /// <summary>
        /// Members the specified member identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Returns member transfer page.</returns>
        [HttpGet]
        public ViewResult Member(int memberId)
        {
            var model = new TransferMemberViewModel
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
        public JsonResult Member(TransferMemberViewModel model)
        {
            return Json(new
            {
                ErrCode = Result.SuccessValue
            });
        }
    }
}