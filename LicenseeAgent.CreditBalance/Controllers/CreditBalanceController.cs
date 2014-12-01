namespace LicenseeAgent.CreditBalance.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CreditBalance.Criteria;
    using LicenseeAgent.CreditBalance.Services;
    using LicenseeAgent.CreditBalance.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// CreditBalanceController class.
    /// </summary>
    [LegendType(LegendType.Credit)]
    public class CreditBalanceController : BaseController
    {
        #region Variables

        private readonly ICreditBalanceService _creditBalanceService;
        private readonly IMemberService _memberService;
        private readonly IDownLinePermissionService _downLinePermissionService;

        #endregion Variables

        #region Constructors

        public CreditBalanceController()
            : this(null, null, null)
        {
        }

        public CreditBalanceController(
            ICreditBalanceService creditBalanceService,
            IMemberService memberService,
            IDownLinePermissionService downLinePermissionService)
        {
            _creditBalanceService = creditBalanceService ?? new CreditBalanceService();
            _memberService = memberService ?? new MemberService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
        }

        #endregion Constructors

        #region Index

        [HttpGet]
        public ActionResult Index()
        {
            var model = new CreditBalanceViewModel
            {
                Status = (int)AccountStatus.Open
            };
            return View(model);
        }

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns list of customer balance.</returns>
        [HttpPost]
        public JsonResult Index(CreditBalanceViewModel model)
        {
            return SessionManager.CurrentSession.AgentInfo.IsLastLevel
                ? SearchMember(model)
                : SearchAgent(model);
        }

        /// <summary>
        /// Searches the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON balance.</returns>
        [NonAction]
        public JsonResult SearchAgent(CreditBalanceViewModel model)
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
        /// Searches the member.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON balance.</returns>
        [NonAction]
        public JsonResult SearchMember(CreditBalanceViewModel model)
        {
            var memberBalanceInfo = _creditBalanceService.GetMemberBalanceInfo(new GetMemberBalanceInfoCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                Username = model.UserName,
                IsYBFiltered = false
            });

            _memberService.GetMemberInfoList(
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

        #endregion Index

        #region Edit agent credit

        /// <summary>
        /// Edit agent credit.
        /// </summary>
        /// <param name="model">Edit agent credit view model.</param>
        /// <returns>
        /// The EditAgentCredit view.
        /// </returns>
        [HttpGet]
        public ActionResult EditAgent(EditAgentViewModel model)
        {
            if (!_downLinePermissionService.IsDirectDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedPage();
            }

            var isAgentLevel0 = model.AgentLevel == 0
                ? SessionManager.CurrentSession.AgentInfo.IsLevel0
                : ModelHelper.IsAgentLevel1(model.AgentLevel);

            if (isAgentLevel0)
            {
                var licenseeGetAgentRangeInfoCriteria = new LicenseeGetAgentRangeInfoCriteria
                {
                    AgentId = model.AgentId
                };
                _creditBalanceService.LicenseeGetAgentRangeInfo(licenseeGetAgentRangeInfoCriteria);
                model.MinCreditOutput = licenseeGetAgentRangeInfoCriteria.MinCreditOutput;
                model.MaxCreditOutput = licenseeGetAgentRangeInfoCriteria.MaxCreditOutput;
                model.GivenCredit = licenseeGetAgentRangeInfoCriteria.GivenCredit;
                model.AgentName = licenseeGetAgentRangeInfoCriteria.Username;
            }
            else
            {
                var agentGetAgentRangeInfoCriteria = new AgentGetAgentRangeInfoCriteria
                {
                    AgentId = model.AgentId
                };
                _creditBalanceService.AgentGetAgentRangeInfo(agentGetAgentRangeInfoCriteria);
                model.MinCreditOutput = agentGetAgentRangeInfoCriteria.MinCreditOutput;
                model.MaxCreditOutput = agentGetAgentRangeInfoCriteria.MaxCreditOutput;
                model.GivenCredit = agentGetAgentRangeInfoCriteria.GivenCredit;
                model.AgentName = agentGetAgentRangeInfoCriteria.Username;
            }

            return View(model);
        }

        /// <summary>
        /// Edit agent credit post.
        /// </summary>
        /// <param name="model">Edit agent credit view model.</param>
        /// <returns>
        /// The EditAgentCredit view.
        /// </returns>
        [HttpPost, ActionName("EditAgent")]
        public ActionResult EditAgentPost(EditAgentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_downLinePermissionService.IsDirectDownLineAgent(model.AgentId))
                {
                    return RedirectToAccessDeniedPage();
                }

                string settingKey = SessionManager.CurrentSession.AgentInfo.IsLevel0 ? "Las_LicenseeUpdAgentCredit" : "Las_AgentUpdAgentCredit";

                var criteria = new UpdateAgentCreditCriteria(settingKey)
                {
                    AgentId = model.AgentId,
                    Credit = model.GivenCredit,
                    IP = Request.GetClientIp(),
                    Updater = CurrentSession.LoginUserId
                };
                _creditBalanceService.UpdateAgentCredit(criteria);

                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(criteria.ErrCode.ToString(), GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ViewBag.Result = Las.UpdateSuccessfully;
                    if (model.IsAccountInfo)
                    {
                        ViewBag.IsAccountInfo = true;
                    }
                }
            }

            return View(model);
        }

        #endregion Edit agent credit

        #region Edit member credit

        /// <summary>
        /// Agent the edit member credit.
        /// </summary>
        /// <param name="model">The agent edit member credit view model.</param>
        /// <returns>
        /// The AgentEditMemberCredit view.
        /// </returns>
        [HttpGet]
        public ActionResult EditMemberCredit(EditMemberViewModel model)
        {
            if (!_downLinePermissionService.IsDirectDownLineMember(model.CustId))
            {
                return RedirectToAccessDeniedPage();
            }

            var agentGetMemberRangeInfoCriteria = new AgentGetMemberRangeInfoCriteria
            {
                CustId = model.CustId
            };

            _creditBalanceService.AgentGetMemberRangeInfo(agentGetMemberRangeInfoCriteria);
            model.MinCreditOutput = agentGetMemberRangeInfoCriteria.MinCreditOutput;
            model.MaxCreditOutput = agentGetMemberRangeInfoCriteria.MaxCreditOutput;
            model.GivenCredit = agentGetMemberRangeInfoCriteria.GivenCredit;
            model.MemberName = agentGetMemberRangeInfoCriteria.Username;

            return View(model);
        }

        /// <summary>
        /// Agent the edit member credit.
        /// </summary>
        /// <param name="model">The agent edit member credit view model.</param>
        /// <returns>
        /// The AgentEditMemberCredit view.
        /// </returns>
        [HttpPost, ActionName("EditMemberCredit")]
        public ActionResult EditMemberCreditPost(EditMemberViewModel model)
        {
            if (!_downLinePermissionService.IsDirectDownLineMember(model.CustId))
            {
                return RedirectToAccessDeniedPage();
            }

            if (ModelState.IsValid)
            {
                var criteria = new AgentUpdateMemberCreditCriteria
                {
                    CustId = model.CustId,
                    Credit = model.GivenCredit,
                    IP = Request.GetClientIp(),
                    Updater = CurrentSession.LoginUserId
                };
                _creditBalanceService.AgentUpdateMemberCredit(criteria);

                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(criteria.ErrCode.ToString(), GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ViewBag.Result = Las.UpdateSuccessfully;
                    if (model.IsAccountInfo)
                    {
                        ViewBag.IsAccountInfo = true;
                    }
                }
            }

            return View(model);
        }

        #endregion Edit member credit
    }
}