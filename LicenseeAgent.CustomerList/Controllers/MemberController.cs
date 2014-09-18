namespace LicenseeAgent.Customer.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Customer.Criteria;
    using LicenseeAgent.Customer.Services;
    using LicenseeAgent.Customer.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// MemberController class.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Review")]
    public class MemberController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IMemberService _memberService;
        private readonly IDownLinePermissionService _downLinePermissionService;

        public MemberController()
            : this(null, null, null)
        {
        }

        public MemberController(
            ICustomerService customerService,
            IMemberService memberService,
            IDownLinePermissionService downLinePermissionService)
        {
            _customerService = customerService ?? new CustomerService();
            _memberService = memberService ?? new MemberService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
        }

        /// <summary>
        /// Change member account status.
        /// </summary>
        /// <param name="model">Member status view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public JsonResult ChangeStatus(MemberStatusViewModel model)
        {
            if (!_downLinePermissionService.IsDirectDownLineMember(model.MemberId))
            {
                return RedirectToAccessDeniedJson();
            }

            if (model.Status == (int)AccountStatus.Open)
            {
                var criteria = new CheckMemberUpLineStatusForUpdateCriteria
                {
                    CustId = model.MemberId
                };
                _customerService.CheckMemberUplineStatusForUpdate(criteria);
                if (criteria.ErrCode != Result.SuccessValue)
                {
                    return JsonResult(criteria.ErrCode);
                }
            }

            var serviceResult = _memberService.UpdateMemberStatus(new UpdateMemberStatusAction
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.LoginUserId,
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                IBCCustId = model.MemberId.ToString(),
                IsClosed = model.Status == (int)AccountStatus.Closed,
                LASAgentId = model.AgentId.ToString()
            });

            return JsonResult(serviceResult);
        }

        /// <summary>
        /// Reset Password.
        /// </summary>
        /// <param name="memberId">Member Id.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult ResetPassword(int memberId)
        {
            if (!_downLinePermissionService.IsDownLineMember(memberId))
            {
                return RedirectToAccessDeniedPage();
            }

            return View(new ResetPasswordViewModel
                {
                    CustId = memberId
                });
        }

        /// <summary>
        /// Reset Password Member.
        /// </summary>
        /// <param name="model">Reset Password View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_downLinePermissionService.IsDownLineMember(model.CustId))
                {
                    return RedirectToAccessDeniedPage();
                }

                // Call API to update member password
                var serviceResult = _memberService.ResetPassword(new ResetPasswordAction
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    UserId = CurrentSession.LoginUserId,
                    IBCCustId = model.CustId,
                    Password = model.Password,
                    SiteName = CurrentSession.AgentInfo.LicenseeName
                });

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, serviceResult.ErrMsg);
                }
                else
                {
                    ViewBag.Result = Las.UpdateSuccessfully;
                }
            }

            return View();
        }

        /// <summary>
        /// Edit Info Member.
        /// </summary>
        /// <param name="memberId">Member Id.</param>
        /// <param name="username">Username member.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [LegendType(LegendType.Credit)]
        public ActionResult EditInfo(int memberId, string username)
        {
            if (!_downLinePermissionService.IsDirectDownLineMember(memberId))
            {
                return RedirectToAccessDeniedPage();
            }

            var memberInfo = new CustomerEntity { CustId = memberId };

            _memberService.GetMemberInformation(
                memberInfo,
                new GetMemberInfoAction
                {
                    IBCCustId = memberId,
                    SiteName = CurrentSession.AgentInfo.LicenseeName
                });

            return View(
                "~/Views/CustomerList/EditInfo.cshtml",
                new EditInfoViewModel
                {
                    CustId = memberId,
                    FirstName = memberInfo.FirstName,
                    LastName = memberInfo.LastName,
                    Mobile = memberInfo.Mobile,
                    Phone = memberInfo.Phone,
                    Fax = memberInfo.Fax,
                    UserName = username
                });
        }

        /// <summary>
        /// Reset Password Member.
        /// </summary>
        /// <param name="model">Reset Password View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [LegendType(LegendType.Credit)]
        public ActionResult EditInfo(EditInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_downLinePermissionService.IsDirectDownLineMember(model.CustId))
                {
                    return RedirectToAccessDeniedPage();
                }

                // Call API to update member password
                var serviceResult = _memberService.UpdateMemberInfo(new UpdateMemberInfoAction
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    UserId = CurrentSession.LoginUserId,
                    IBCCustId = model.CustId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    OtherPhone = model.Mobile,
                    Fax = model.Fax,
                    SiteName = CurrentSession.AgentInfo.LicenseeName
                });

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, serviceResult.ErrMsg);
                }
                else
                {
                    ViewBag.Result = Las.UpdateSuccessfully;
                }
            }

            return View("~/Views/CustomerList/EditInfo.cshtml", model);
        }

        /// <summary>
        /// Up lines the agents by customer identifier.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult UplineAgentsByCustId(int custId)
        {
            var criteria = new GetUplineByCustIdCriteria
            {
                CustId = custId
            };
            var listAgent = _customerService.UplineAgentsByCustId(criteria);

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = listAgent
            });
        }
    }
}