// --------------------------------------------------------------------------
// <copyright file="CreditBalanceController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CreditBalance.Criteria;
    using LicenseeAgent.CreditBalance.Entities;
    using LicenseeAgent.CreditBalance.Services;
    using LicenseeAgent.CreditBalance.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CreditBalanceController : BaseController
    {
        private readonly ICreditBalanceService _creditBalanceService;
        private readonly IMemberService _memberService;
        private readonly IDownlLinePermissionService _permissionService;

        public CreditBalanceController(
            ICreditBalanceService creditBalanceService,
            IMemberService memberService,
            IDownlLinePermissionService permissionService)
            : base(permissionService)
        {
            _creditBalanceService = creditBalanceService;
            _memberService = memberService;
            _permissionService = permissionService;
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
        public JsonResult Search(CreditBalanceSearchViewModel model)
        {
            if (SessionManager.CurrentSession.AgentInfo.IsLastLevel)
            {
                return SearchMember(model);
            }
            else
            {
                return SearchAgent(model);
            }
        }

        /// <summary>
        /// Searches the member.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON balance.</returns>
        [NonAction]
        public JsonResult SearchMember(CreditBalanceSearchViewModel model)
        {
            var memberBalanceInfo = _creditBalanceService.GetMemberBalanceInfo(new GetMemberBalanceInfoCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                Username = model.UserName,
                IsYBFiltered = false
            });

            _memberService.GetMemberInformations<MemberBalanceInfo>(
                                memberBalanceInfo,
                                new GetMemberInfoListAction
                                {
                                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                                    LASAgentId = CurrentSession.AgentInfo.AgentId
                                });
            memberBalanceInfo = memberBalanceInfo.Where(m => !model.Status.HasValue || (m.Status != Convert.ToBoolean(model.Status.Value))).ToList();

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = memberBalanceInfo
            });
        }

        /// <summary>
        /// Searches the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON balance.</returns>
        [NonAction]
        public JsonResult SearchAgent(CreditBalanceSearchViewModel model)
        {
            var agentBalanceInfo = _creditBalanceService.GetAgentBalanceInfo(new GetAgentBalanceInfoCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                Username = model.UserName,
                Status = model.Status,
                IsYBFiltered = false
            });

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = agentBalanceInfo
            });
        }

        /// <summary>
        /// Edit agent credit.
        /// </summary>
        /// <param name="editAgentCreditViewModel">Edit agent credit view model.</param>
        /// <returns>
        /// The EditAgentCredit view.
        /// </returns>
        [HttpGet]
        public ActionResult EditAgentCredit(EditAgentCreditViewModel editAgentCreditViewModel)
        {
            if (!_permissionService.IsDirectDownLineAgent(editAgentCreditViewModel.AgentId))
            {
                return RedirectToAccessDeniedPage();
            }

            var isAgentLevel0 = editAgentCreditViewModel.AgentLevel == 0
                                ? SessionManager.CurrentSession.AgentInfo.IsLevel0
                                : ModelHelper.IsAgentLevel0(editAgentCreditViewModel.AgentLevel);

            if (isAgentLevel0)
            {
                var licenseeGetAgentRangeInfoCriteria = new LicenseeGetAgentRangeInfoCriteria
                {
                    AgentId = editAgentCreditViewModel.AgentId
                };
                _creditBalanceService.LicenseeGetAgentRangeInfo(licenseeGetAgentRangeInfoCriteria);
                editAgentCreditViewModel.MinCreditOutput = licenseeGetAgentRangeInfoCriteria.MinCreditOutput;
                editAgentCreditViewModel.MaxCreditOutput = licenseeGetAgentRangeInfoCriteria.MaxCreditOutput;
            }
            else
            {
                var agentGetAgentRangeInfoCriteria = new AgentGetAgentRangeInfoCriteria
                {
                    AgentId = editAgentCreditViewModel.AgentId
                };
                _creditBalanceService.AgentGetAgentRangeInfo(agentGetAgentRangeInfoCriteria);
                editAgentCreditViewModel.MinCreditOutput = agentGetAgentRangeInfoCriteria.MinCreditOutput;
                editAgentCreditViewModel.MaxCreditOutput = agentGetAgentRangeInfoCriteria.MaxCreditOutput;
            }

            return View(editAgentCreditViewModel);
        }

        /// <summary>
        /// Edit agent credit post.
        /// </summary>
        /// <param name="editAgentCreditViewModel">Edit agent credit view model.</param>
        /// <returns>The EditAgentCredit view.</returns>
        [HttpPost, ActionName("EditAgentCredit")]
        public ActionResult EditAgentCreditPost(EditAgentCreditViewModel editAgentCreditViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_permissionService.IsDirectDownLineAgent(editAgentCreditViewModel.AgentId))
                {
                    return RedirectToAccessDeniedPage();
                }

                string settingKey = SessionManager.CurrentSession.AgentInfo.IsLevel0 ? "Las_LicenseeUpdAgentCredit" : "Las_AgentUpdAgentCredit";

                var criteria = new UpdateAgentCreditCriteria(settingKey)
                {
                    AgentId = editAgentCreditViewModel.AgentId,
                    Credit = editAgentCreditViewModel.GivenCredit,
                    Ip = Request.GetClientIp(),
                    Updater = CurrentSession.LoginUserId
                };
                _creditBalanceService.UpdateAgentCredit(criteria);

                if (criteria.MsgId != Result.SuccessValue)
                {
                    ModelState.AddModelError(criteria.MsgId.ToString(), GetErrMsg(criteria.MsgId));
                }
                else
                {
                    ViewBag.Result = Las.UpdateSuccessfully;
                    if (editAgentCreditViewModel.IsAccountInfo)
                    {
                        ViewBag.IsAccountInfo = true;
                    }
                }
            }

            return View(editAgentCreditViewModel);
        }

        /// <summary>
        /// Agent the edit member credit.
        /// </summary>
        /// <param name="agentEditMemberCreditViewModel">The agent edit member credit view model.</param>
        /// <returns>
        /// The AgentEditMemberCredit view.
        /// </returns>
        [HttpGet]
        public ActionResult AgentEditMemberCredit(AgentEditMemberCreditViewModel agentEditMemberCreditViewModel)
        {
            if (!_permissionService.IsDirectDownLineMember(agentEditMemberCreditViewModel.CustId))
            {
                return RedirectToAccessDeniedPage();
            }

            var agentGetMemberRangeInfoCriteria = new AgentGetMemberRangeInfoCriteria
            {
                CustId = agentEditMemberCreditViewModel.CustId
            };

            _creditBalanceService.AgentGetMemberRangeInfo(agentGetMemberRangeInfoCriteria);
            agentEditMemberCreditViewModel.MinCreditOutput = agentGetMemberRangeInfoCriteria.MinCreditOutput;
            agentEditMemberCreditViewModel.MaxCreditOutput = agentGetMemberRangeInfoCriteria.MaxCreditOutput;

            return View(agentEditMemberCreditViewModel);
        }

        /// <summary>
        /// Agent the edit member credit.
        /// </summary>
        /// <param name="agentEditMemberCreditViewModel">The agent edit member credit view model.</param>
        /// <returns>
        /// The AgentEditMemberCredit view.
        /// </returns>
        [HttpPost, ActionName("AgentEditMemberCredit")]
        public ActionResult AgentEditMemberCreditPost(AgentEditMemberCreditViewModel agentEditMemberCreditViewModel)
        {
            if (!_permissionService.IsDirectDownLineMember(agentEditMemberCreditViewModel.CustId))
            {
                return RedirectToAccessDeniedPage();
            }

            if (ModelState.IsValid)
            {
                var criteria = new AgentUpdateMemberCreditCriteria
                {
                    CustId = agentEditMemberCreditViewModel.CustId,
                    Credit = agentEditMemberCreditViewModel.GivenCredit,
                    Ip = Request.GetClientIp(),
                    Updater = CurrentSession.LoginUserId
                };
                _creditBalanceService.AgentUpdateMemberCredit(criteria);

                if (criteria.MsgId != Result.SuccessValue)
                {
                    ModelState.AddModelError(criteria.MsgId.ToString(), GetErrMsg(criteria.MsgId));
                }
                else
                {
                    ViewBag.Result = Las.UpdateSuccessfully;
                    if (agentEditMemberCreditViewModel.IsAccountInfo)
                    {
                        ViewBag.IsAccountInfo = true;
                    }
                }
            }

            return View(agentEditMemberCreditViewModel);
        }
    }
}