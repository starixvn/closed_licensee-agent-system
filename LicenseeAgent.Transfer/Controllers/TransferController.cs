// --------------------------------------------------------------------------
// <copyright file="TransferController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Transfer.Criteria;
    using LicenseeAgent.Transfer.Entities;
    using LicenseeAgent.Transfer.Services;
    using LicenseeAgent.Transfer.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferController : BaseController
    {
        private readonly ITransferService _transferService;
        private readonly IMemberService _memberService;

        public TransferController(ITransferService transferService, IMemberService memberService)
        {
            _transferService = transferService;
            _memberService = memberService;
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
            if (CurrentSession.AgentInfo.IsLastLevel)
            {
                return SearchMember(model);
            }
            else
            {
                return SearchAgent(model);
            }
        }

        /// <summary>
        /// Searches the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON balance.</returns>
        [NonAction]
        public JsonResult SearchAgent(TransferSearchViewModel model)
        {
            var list = _transferService.GetAgentBalanceInfo(new GetAgentBalanceInfoCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    Username = model.UserName,
                    Status = model.Status
                });

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = list
            });
        }

        /// <summary>
        /// Searches the member.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON balance.</returns>
        [NonAction]
        public JsonResult SearchMember(TransferSearchViewModel model)
        {
            var list = _transferService.GetMemberBalanceInfo(new GetMemberBalanceInfoCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    Username = model.UserName
                });

            var response = _memberService.GetMemberInformations<MemberBalanceInfo>(
                list,
                new GetMemberInfoListAction
                {
                    LASAgentId = CurrentSession.AgentInfo.AgentId,
                    SiteName = CurrentSession.AgentInfo.LicenseeName
                });

            if (response.ErrCode != Result.SuccessValue)
            {
                return Json(response);
            }

            list = list.Where(x => x.UserName.Contains(model.UserName, StringComparison.OrdinalIgnoreCase) &&
                (model.Status == null || x.Status == model.Status));
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = list
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
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns member transfer page.
        /// </returns>
        [HttpGet]
        public ViewResult Member(TransferMemberViewModel model)
        {
            var criteria = new GetMemberDetailCriteria
            {
                CustId = model.MemberId
            };

            var memberDetail = _transferService.GetMemberDetail(criteria);
            model.UserName = memberDetail.UserName;
            model.BetCredit = memberDetail.BetCredit;
            model.LastLogin = memberDetail.LastLogin;
            model.LastTransaction = memberDetail.LastTransaction;
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
        /// Members the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns View.</returns>
        [HttpPost, ActionName("Member")]
        [ValidateAntiForgeryToken]
        public ActionResult MemberPost(TransferMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new TransferToMemberCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    ToCustId = model.MemberId,
                    Amount = model.Amount
                };
                _transferService.TransferToMember(criteria);
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
    }
}