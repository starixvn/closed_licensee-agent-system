namespace LicenseeAgent.CustomerList.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerList.Criteria.Agent;
    using LicenseeAgent.CustomerList.Services;
    using LicenseeAgent.CustomerList.ViewModels;
    using LicenseeAgent.CustomerList.ViewModels.Agent;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Binders;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Services;
    using LicenseeAgent.Web.ViewModels;

    /// <summary>
    /// AgentController class.
    /// </summary>
    public class AgentController : BaseController
    {
        #region Variables

        private readonly IAgentListService _agentListService;
        private readonly IMemberService _memberService;
        private readonly IDownLinePermissionService _downLinePermissionService;
        private readonly ICoreService _coreService;

        #endregion Variables

        #region Constructors

        public AgentController()
            : this(null, null, null, null)
        {
        }

        public AgentController(
            IAgentListService agentListService,
            IMemberService memberService,
            IDownLinePermissionService downLinePermissionService,
            ICoreService coreService)
        {
            _agentListService = agentListService ?? new AgentListService();
            _memberService = memberService ?? new MemberService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _coreService = coreService ?? new CoreService();
        }

        #endregion Constructors

        #region Index

        /// <summary>
        /// Agents the list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ViewResult Index([ModelBinder(typeof(AgentBreadcrumbModelBinder))]AgentBreadcrumbViewModel model)
        {
            var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            ViewBag.PageContext = new PageContext(CurrentSession, model.AgentId, model.AgentLevel)
            {
                IsOfferedVirtualSports = _coreService.HasProductOffered(productInfos, (int)ProductCode.Product_5),
                IsOfferedKeno = _coreService.HasProductOffered(productInfos, (int)ProductCode.Product_8),
                IsMember = false
            };

            return View(model);
        }

        /// <summary>
        /// Get agent list.
        /// </summary>
        /// <param name="model">Agent search view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public JsonResult Index(CustomerListViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var isGetAll = string.IsNullOrEmpty(model.UserName) && model.Status == null;
            return isGetAll ? GetAgents(model) : SearchAgents(model);
        }

        /// <summary>
        /// Get agent by search.
        /// </summary>
        /// <param name="model">Agent search view model.</param>
        /// <returns>Action result.</returns>
        [NonAction]
        public JsonResult GetAgents(CustomerListViewModel model)
        {
            var criteria = new AgentGetByAgentCriteria
            {
                AgentId = model.AgentId,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex
            };

            var agents = _agentListService.GetAgentList(criteria);
            return Json(new
            {
                ErrCode = criteria.MsgId,
                ErrMsg = GetErrMsg(criteria.MsgId),
                List = agents,
                CurrentPage = criteria.PageIndex,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.TotalItems
            });
        }

        /// <summary>
        /// Get agent by search.
        /// </summary>
        /// <param name="model">Agent search view model.</param>
        /// <returns>Action result.</returns>
        [NonAction]
        public JsonResult SearchAgents(CustomerListViewModel model)
        {
            var criteria = new SearchAgentCriteria
            {
                AgentId = model.AgentId,
                UserName = model.UserName,
                AccStatus = model.Status,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex
            };

            var agents = _agentListService.SearchAgent(criteria);
            return Json(new
            {
                ErrCode = criteria.MsgId,
                ErrMsg = GetErrMsg(criteria.MsgId),
                List = agents,
                CurrentPage = criteria.PageIndex,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.TotalItems
            });
        }

        #endregion Index

        #region Auto-complete

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

            var agents = _agentListService.GetAgentAutoComplete(new AgentAutoCompleteCriteria
            {
                AgentId = model.AgentId,
                Text = model.UserName
            });
            var userNames = agents.Select(m => m.UserName);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = userNames
            });
        }

        #endregion Auto-complete

        #region Change status

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
            var agentsHasMember = _agentListService.UpdateAgentAccStatus(criteria);
            if (criteria.MsgId == Result.SuccessValue && model.Status == (int)AccountStatus.Closed && agentsHasMember.Count > 0)
            {
                var serviceResult = _memberService.UpdateMemberStatus(new UpdateMemberStatusAction
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    UserId = CurrentSession.LoginUserId,
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    LASAgentId = string.Join(",", agentsHasMember.Select(a => a.AgentId.ToString()).ToArray()),
                    IsClosed = true
                });

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    return JsonResult(serviceResult);
                }
            }

            return JsonResult(criteria.MsgId);
        }

        #endregion Change status

        #region Reset password

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

                var criteria = new ResetPasswordAgentCriteria
                {
                    AgentId = model.CustId,
                    Password = model.Password,
                    UserId = CurrentSession.LoginUserId,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    UserSalt = Cryptography.CreateSalt()
                };

                _agentListService.ResetPassword(criteria);

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

        #endregion Reset password

        #region Edit information

        /// <summary>
        /// Edit information.
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

            var agentInfo = _agentListService.GetAgentInfo(new AgentInfoCriteria
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

                _agentListService.EditInfo(criteria);

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

        #endregion Edit information
    }
}