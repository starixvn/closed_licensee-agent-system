namespace LicenseeAgent.CustomerList.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerList.Criteria.Member;
    using LicenseeAgent.CustomerList.Entities.Member;
    using LicenseeAgent.CustomerList.Services;
    using LicenseeAgent.CustomerList.ViewModels;
    using LicenseeAgent.CustomerList.ViewModels.Member;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Enumerations;
    using LicenseeAgent.Taiwan.Filter;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Taiwan.Utility;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Binders;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Services;
    using LicenseeAgent.Web.ViewModels;

    /// <summary>
    /// MemberController class.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Review")]
    public class MemberController : BaseController
    {
        #region Variables

        private readonly IMemberListService _memberListService;
        private readonly IMemberService _memberService;
        private readonly IDownLinePermissionService _downLinePermissionService;
        private readonly ICoreService _coreService;

        #endregion Variables

        #region Constructors

        public MemberController()
            : this(null, null, null, null)
        {
        }

        public MemberController(
            IMemberListService memberListService,
            IMemberService memberService,
            IDownLinePermissionService downLinePermissionService,
            ICoreService coreService)
        {
            _memberListService = memberListService ?? new MemberListService();
            _memberService = memberService ?? new MemberService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _coreService = coreService ?? new CoreService();
        }

        #endregion Constructors

        #region Index

        /// <summary>
        /// Members the list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Page member list.</returns>
        [HttpGet]
        public ViewResult Index([ModelBinder(typeof(AgentBreadcrumbModelBinder))]AgentBreadcrumbViewModel model)
        {
            var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            ViewBag.PageContext = new PageContext(CurrentSession, model.AgentId, model.AgentLevel)
            {
                IsOfferedVirtualSports = _coreService.HasProductOffered(productInfos, (int)ProductCode.Product_5),
                IsMember = true
            };

            return View(model);
        }

        /// <summary>
        /// Indexes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Index page.</returns>
        [HttpPost]
        public JsonResult Index(CustomerListViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var serviceResult = GetMemberInformations(model);
            if (serviceResult.ErrCode != Result.SuccessValue)
            {
                return JsonResult(serviceResult);
            }

            var serviceMembers = new List<DepositMemberInfo>();
            if (serviceResult.Data != null && serviceResult.Data.Any())
            {
                serviceMembers = serviceResult.Data.Where(m => model.Status == null || m.Status == model.Status)
                    .OrderBy(m => m.Username).Skip(model.PageIndex * model.PageSize).Take(model.PageSize).ToList();
            }

            if (!serviceMembers.Any())
            {
                return Json(new
                {
                    List = new List<MemberEntity>(),
                    CurrentPage = model.PageIndex,
                    ItemsPerPage = model.PageSize,
                    TotalItem = 0
                });
            }

            var members = _memberListService.SearchMembersByIds(new SearchMemberByIdsCriteria
            {
                AgentId = model.AgentId,
                ListCustId = string.Join(",", serviceMembers.Select(m => m.IBCCustId.ToString()).ToArray())
            });
            ServiceMapper.Map(serviceMembers, members, new MemberFilter());
            if (!CurrentSession.IsAdmin)
            {
                members.ToList().ForEach(r => r.SystemId = string.Empty);
            }

            return Json(new
            {
                List = members,
                CurrentPage = model.PageIndex,
                ItemsPerPage = model.PageSize,
                TotalItem = members.Count()
            });
        }

        /// <summary>
        /// Get member information.
        /// </summary>
        /// <param name="model">Customer list view model.</param>
        /// <returns>Member list result.</returns>
        [NonAction]
        public GetDepositMemberInfoListResult GetMemberInformations(CustomerListViewModel model)
        {
            return _memberService.GetDepositMemberInfoList(new GetDepositMemberInfoListAction
            {
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                LASAgentId = model.AgentId,
                DepositOption = (int)DepositOption.DepositOnly,
                Username = model.UserName
            });
        }

        #endregion Index

        #region Auto complete

        /// <summary>
        /// Get agent auto complete.
        /// </summary>
        /// <param name="model">Customer auto complete view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public JsonResult AutoComplete(CustomerSearchAutoCompleteViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var members = _memberListService.GetMemberAutoComplete(new MemberAutoCompleteCriteria
            {
                AgentId = model.AgentId,
                Text = model.UserName
            });
            var userNames = members.Select(m => m.UserName);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = userNames
            });
        }

        #endregion Auto complete

        #region Change status

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
                _memberListService.CheckMemberUplineStatusForUpdate(criteria);
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

        #endregion Change status

        #region Reset password

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

        #endregion Reset password

        #region Edit information

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

        #endregion Edit information

        #region Get Upline agents

        /// <summary>
        /// Up line of the agents.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// List Up line of agents.
        /// </returns>
        [HttpPost]
        public ViewResult DirectUpLines(DirectUpLinesViewModel model)
        {
            var listUplines = _memberListService.UplineAgentsByCustId(new GetUplineByCustIdCriteria
            {
                CustId = model.CustId
            });

            model.AgentList = listUplines;
            return View(model);
        }

        #endregion Get Upline agents
    }
}