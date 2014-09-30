namespace LicenseeAgent.Customer.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Customer.Criteria;
    using LicenseeAgent.Customer.Services;
    using LicenseeAgent.Customer.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// AgentController class.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Review")]
    public class AgentController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IMemberService _memberService;
        private readonly IDownLinePermissionService _downLinePermissionService;

        public AgentController()
            : this(null, null, null)
        {
        }

        public AgentController(
            ICustomerService customerService,
            IMemberService memberService,
            IDownLinePermissionService downLinePermissionService)
        {
            _customerService = customerService ?? new CustomerService();
            _memberService = memberService ?? new MemberService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
        }

        /// <summary>
        /// Change agent account status.
        /// </summary>
        /// <param name="model">Agent status view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public JsonResult ChangeStatus(AgentStatusViewModel model)
        {
            if (!_downLinePermissionService.IsDirectDownLineAgent(model.AgentId)
                || model.AgentId == CurrentSession.AgentInfo.AgentId)
            {
                return RedirectToAccessDeniedJson();
            }

            var criteria = new UpdateAgentStatusCriteria
            {
                Updater = CurrentSession.LoginUserId,
                AgentId = model.AgentId,
                AccStatus = model.Status
            };

            var agents = _customerService.UpdateAgentAccStatus(criteria);

            if (criteria.MsgId == Result.SuccessValue && model.Status == (int)AccountStatus.Closed)
            {
                var serviceResult = _memberService.UpdateMemberStatus(new UpdateMemberStatusAction
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    UserId = CurrentSession.LoginUserId,
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    LASAgentId = string.Join(",", agents.Select(a => a.AgentId.ToString()).ToArray()),
                    IsClosed = true
                });

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    return JsonResult(serviceResult);
                }
            }

            return JsonResult(criteria.MsgId);
        }

        /// <summary>
        /// Reset Password.
        /// </summary>
        /// <param name="agentId">Agent Id information.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult ResetPassword(int agentId)
        {
            if (!_downLinePermissionService.IsDownLineAgent(agentId))
            {
                return RedirectToAccessDeniedPage();
            }

            return View(new ResetPasswordViewModel
                {
                    CustId = agentId
                });
        }

        /// <summary>
        /// Reset Password Agent.
        /// </summary>
        /// <param name="model">Reset Password View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_downLinePermissionService.IsDownLineAgent(model.CustId))
                {
                    return RedirectToAccessDeniedPage();
                }

                var criteria = new ResetPasswordCriteria
                {
                    AgentId = model.CustId,
                    Password = model.Password,
                    UserId = CurrentSession.LoginUserId,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    UserSalt = Cryptography.CreateSalt()
                };

                _customerService.ResetPassword(criteria);

                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ViewBag.Result = Las.UpdateSuccessfully;
                }
            }

            return View();
        }

        /// <summary>
        /// Reset Password.
        /// </summary>
        /// <param name="agentId">Agent Id information.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult EditInfo(int agentId)
        {
            if (!_downLinePermissionService.IsDirectDownLineAgent(agentId))
            {
                return RedirectToAccessDeniedPage();
            }

            var agentInfo = _customerService.GetAgentInfo(new AgentInfoCriteria
            {
                AgentId = agentId,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.LoginUserId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId
            });

            return View(new EditInfoViewModel
                {
                    CustId = agentInfo.AgentId,
                    FirstName = agentInfo.FirstName,
                    LastName = agentInfo.LastName,
                    Mobile = agentInfo.Mobile,
                    Phone = agentInfo.Phone,
                    Fax = agentInfo.Fax,
                    UserName = agentInfo.UserName
                });
        }

        /// <summary>
        /// Reset Password Agent.
        /// </summary>
        /// <param name="model">Reset Password View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult EditInfo(EditInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_downLinePermissionService.IsDirectDownLineAgent(model.CustId))
                {
                    return RedirectToAccessDeniedPage();
                }

                var criteria = new EditinfoAgentCriteria
                {
                    AgentId = model.CustId,
                    UserId = CurrentSession.LoginUserId,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    Mobile = model.Mobile,
                    Fax = model.Fax
                };

                _customerService.EditInfoAgent(criteria);

                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ViewBag.Result = Las.UpdateSuccessfully;
                }
            }

            return View(model);
        }

        /// <summary>
        /// Up line of the agents.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="product">The product.</param>
        /// <returns>List Up line of agents.</returns>
        public ActionResult UplineAgents(int custId, string product)
        {
            var listUplines = _customerService.UplineAgentsByCustId(new GetUplineByCustIdCriteria
            {
                CustId = custId
            });

            var model = new UplineViewModel
            {
                AgentList = listUplines,
                Product = product,
                CustId = custId
            };
            return View(model);
        }
    }
}