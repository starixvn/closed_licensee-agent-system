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
    using LicenseeAgent.CreditBalance.Criteria;
    using LicenseeAgent.CreditBalance.Entities;
    using LicenseeAgent.CreditBalance.Services;
    using LicenseeAgent.CreditBalance.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CreditBalanceController : BaseController
    {
        private readonly ICreditBalanceService _creditBalanceService;
        private readonly IMemberService _memberService;

        public CreditBalanceController(ICreditBalanceService creditBalanceService, IMemberService memberService)
        {
            _creditBalanceService = creditBalanceService;
            _memberService = memberService;
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
                Username = model.UserName
            });

            var pagedList = memberBalanceInfo.Skip(model.ItemsPerPage * model.PageIndex).Take(model.ItemsPerPage);

            var totalData = new MemberBalanceInfo
            {
                UserName = "Total",
                GivenCredit = pagedList.Select(x => x.GivenCredit).Sum(),
                BetCredit = pagedList.Select(x => x.BetCredit).Sum(),
                YesterdayBalance = pagedList.Select(x => x.YesterdayBalance).Sum(),
                TotalBalance = pagedList.Select(x => x.TotalBalance).Sum(),
                Outstanding = pagedList.Select(x => x.Outstanding).Sum(),
            };

            var serviceResult = _memberService.GetMemberInformations<MemberBalanceInfo>(
                                pagedList,
                                new GetMemberInfoListAction
                                {
                                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                                    LASAgentId = CurrentSession.AgentInfo.AgentId
                                });

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = pagedList,
                TotalData = totalData,
                TotalRecords = memberBalanceInfo.Count()
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
                Status = model.Status
            });

            var pagedList = agentBalanceInfo.Skip(model.ItemsPerPage * model.PageIndex).Take(model.ItemsPerPage);

            var totalData = new AgentBalanceInfo
            {
                UserName = "Total",
                GivenCredit = pagedList.Select(x => x.GivenCredit).Sum(),
                UsedCredit = pagedList.Select(x => x.UsedCredit).Sum(),
                YesterdayBalance = pagedList.Select(x => x.YesterdayBalance).Sum(),
                TotalBalance = pagedList.Select(x => x.TotalBalance).Sum(),
                YesterdayCash = pagedList.Select(x => x.YesterdayCash).Sum(),
                TotalCash = pagedList.Select(x => x.TotalCash).Sum(),
                Outstanding = pagedList.Select(x => x.Outstanding).Sum(),
            };

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = pagedList,
                TotalData = totalData,
                TotalRecords = agentBalanceInfo.Count()
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
        public ViewResult EditAgentCredit(EditAgentCreditViewModel editAgentCreditViewModel)
        {
            if (SessionManager.CurrentSession.AgentInfo.IsLevel0)
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

            return View("~/Views/Account/CreditBalance/EditAgentCredit.cshtml", editAgentCreditViewModel);
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
                string settingKey = SessionManager.CurrentSession.AgentInfo.IsLevel0 ? "Las_LicenseeUpdAgentCredit" : "Las_AgentUpdAgentCredit";

                var criteria = new UpdateAgentCreditCriteria(settingKey)
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
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
                    ViewBag.Result = Las.EditCreditSuccessfully;
                }
            }

            return View("~/Views/Account/CreditBalance/EditAgentCredit.cshtml", editAgentCreditViewModel);
        }

        /// <summary>
        /// Agent the edit member credit.
        /// </summary>
        /// <param name="agentEditMemberCreditViewModel">The agent edit member credit view model.</param>
        /// <returns>
        /// The AgentEditMemberCredit view.
        /// </returns>
        [HttpGet]
        public ViewResult AgentEditMemberCredit(AgentEditMemberCreditViewModel agentEditMemberCreditViewModel)
        {
            var agentGetMemberRangeInfoCriteria = new AgentGetMemberRangeInfoCriteria
            {
                CustId = agentEditMemberCreditViewModel.CustId
            };

            _creditBalanceService.AgentGetMemberRangeInfo(agentGetMemberRangeInfoCriteria);
            agentEditMemberCreditViewModel.MinCreditOutput = agentGetMemberRangeInfoCriteria.MinCreditOutput;
            agentEditMemberCreditViewModel.MaxCreditOutput = agentGetMemberRangeInfoCriteria.MaxCreditOutput;

            return View("~/Views/Account/CreditBalance/AgentEditMemberCredit.cshtml", agentEditMemberCreditViewModel);
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
                    ViewBag.Result = Las.EditCreditSuccessfully;
                }
            }

            return View("~/Views/Account/CreditBalance/AgentEditMemberCredit.cshtml", agentEditMemberCreditViewModel);
        }
    }
}