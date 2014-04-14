// -----------------------------------------------------------------------
// <copyright file="CustomerListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerList.Criteria;
    using LicenseeAgent.CustomerList.Entities;
    using LicenseeAgent.CustomerList.Services;
    using LicenseeAgent.CustomerList.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Binders;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Services;
    using LicenseeAgent.Web.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerListController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IIpInformationService _ipService;
        private readonly IMemberService _memberService;
        private readonly IDownlLinePermissionService _permissionService;

        public CustomerListController(
            ICustomerService agentService,
            IIpInformationService ipService,
            IMemberService memberService,
            IDownlLinePermissionService permissionService)
            : base(permissionService)
        {
            _customerService = agentService;
            _ipService = ipService;
            _memberService = memberService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// Agent List.
        /// </summary>
        /// <param name="model">Agent information view model.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult Index(AgentInfoViewModel model)
        {
            var hasAgentModel = model.AgentId != 0;

            var breadModel = new AgentBreadcrumViewModel(
                hasAgentModel ? model.AgentId : CurrentSession.AgentInfo.AgentId,
                hasAgentModel ? model.AgentName : CurrentSession.AgentInfo.AgentName,
                hasAgentModel ? model.AgentLevel : CurrentSession.AgentInfo.AgentLevel);

            var isLastLevel = hasAgentModel ? ModelHelper.IsLastAgent(model.AgentLevel, model.NumberOfLevel) : CurrentSession.AgentInfo.IsLastLevel;

            ViewBag.PageContext = new PageContext(breadModel.AgentId, isLastLevel)
            {
                IsDirectDownline = CurrentSession.IsDirectDownline(hasAgentModel ? model.AgentLevel : CurrentSession.AgentInfo.AgentLevel),
                IsAdmin = CurrentSession.IsAdmin
            };

            return View(breadModel);
        }

        /// <summary>
        /// Agents the list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult Index([ModelBinder(typeof(AgentBreadcrumModelBinder))]AgentBreadcrumViewModel model)
        {
            if (!_permissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedPage();
            }

            var breadcrumbItem = new BreadcrumbItem
            {
                AgentId = model.AgentId,
                AgentName = model.AgentName,
                AgentLevel = model.AgentLevel
            };

            if (!model.Breadcrumb.Join(breadcrumbItem))
            {
                return InvalidModelViewResult();
            }

            ViewBag.PageContext = new PageContext(model.AgentId, model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel)
            {
                IsDirectDownline = CurrentSession.IsDirectDownline(model.AgentLevel),
                IsAdmin = CurrentSession.IsAdmin
            };

            return View(model);
        }

        /// <summary>
        /// Get agent list.
        /// </summary>
        /// <param name="model">Agent search view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public JsonResult GetCustomerList(CustomerListViewModel model)
        {
            if (!_permissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var isLastLevel = CurrentSession.AgentInfo.IsLastAgent(model.AgentLevel);
            var isGetAll = string.IsNullOrEmpty(model.UserName) && model.Status == null;
            if (isLastLevel)
            {
                if (isGetAll)
                {
                    return GetMembers(model);
                }
                else
                {
                    return SearchMembers(model);
                }
            }
            else
            {
                if (isGetAll)
                {
                    return GetAgents(model);
                }
                else
                {
                    return SearchAgents(model);
                }
            }
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

            var agents = _customerService.GetAgentList(criteria);
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

            var agents = _customerService.SearchAgent(criteria);
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
        /// Get member list by search.
        /// </summary>
        /// <param name="model">Agent search view model.</param>
        /// <returns>Action result.</returns>
        [NonAction]
        public JsonResult GetMembers(CustomerListViewModel model)
        {
            var criteria = new MemberGetByAgentCriteria
            {
                AgentId = model.AgentId
            };

            var members = _customerService.GetMemberList(criteria);

            if (criteria.MsgId == Result.SuccessValue && members.Any())
            {
                var serviceResult = _memberService.GetMemberInformations<MemberEntity>(
                                    members,
                                    new GetMemberInfoListAction
                                    {
                                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                                        LASAgentId = model.AgentId
                                    });

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    return Json(new
                    {
                        ErrCode = serviceResult.ErrCode,
                        ErrMsg = serviceResult.ErrMsg
                    });
                }
            }

            return Json(new
            {
                ErrCode = criteria.MsgId,
                ErrMsg = GetErrMsg(criteria.MsgId),
                List = members
            });
        }

        /// <summary>
        /// Get member list by search.
        /// </summary>
        /// <param name="model">Agent search view model.</param>
        /// <returns>Action result.</returns>
        [NonAction]
        public JsonResult SearchMembers(CustomerListViewModel model)
        {
            var criteria = new SearchMemberCriteria
            {
                AgentId = model.AgentId,
                UserName = model.UserName,
            };

            var members = _customerService.SearchMember(criteria);

            if (criteria.MsgId == Result.SuccessValue && members.Any())
            {
                var serviceResult = _memberService.GetMemberInformations<MemberEntity>(
                    members,
                    new GetMemberInfoListAction
                    {
                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                        LASAgentId = model.AgentId
                    });

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    return Json(new
                    {
                        ErrCode = serviceResult.ErrCode,
                        ErrMsg = serviceResult.ErrMsg
                    });
                }
            }

            return Json(new
            {
                ErrCode = criteria.MsgId,
                ErrMsg = GetErrMsg(criteria.MsgId),
                List = model.Status == null ? members : members.Where(m => m.Status == model.Status)
            });
        }

        /// <summary>
        /// Change agent account status.
        /// </summary>
        /// <param name="model">Agent status view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public JsonResult ChangeAgentStatus(AgentStatusViewModel model)
        {
            if (!_permissionService.IsDirectDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var criteria = new UpdateAgentAccStatusCriteria
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
                    LASAgentId = string.Join(",", agents.Select(a => a.AgentId.ToString()).ToArray<string>()),
                    IsClosed = true
                });

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    return Json(new
                    {
                        ErrCode = serviceResult.ErrCode,
                        ErrMsg = serviceResult.ErrMsg
                    });
                }
            }

            return JsonResult(criteria.MsgId);
        }

        /// <summary>
        /// Change member account status.
        /// </summary>
        /// <param name="model">Member status view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public JsonResult ChangeMemberStatus(MemberStatusViewModel model)
        {
            if (!_permissionService.IsDirectDownLineMember(model.MemberId))
            {
                return RedirectToAccessDeniedJson();
            }

            if (model.Status == (int)AccountStatus.Open)
            {
                var criteria = new CheckMemberUplineStatusForUpdateCriteria
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
        /// Get agent auto complete.
        /// </summary>
        /// <param name="model">Customer auto complete view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public JsonResult CustomerSearchAutoComplete(CustomerSearchAutoCompleteViewModel model)
        {
            if (!_permissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var isLastLevel = CurrentSession.AgentInfo.IsLastAgent(model.AgentLevel);
            if (isLastLevel)
            {
                return SearchMemberAutoComplete(model);
            }
            else
            {
                return SearchAgentAutoComplete(model);
            }
        }

        /// <summary>
        /// Searches the agent automatic complete.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>JSON result.</returns>
        [NonAction]
        public JsonResult SearchAgentAutoComplete(CustomerSearchAutoCompleteViewModel model)
        {
            var criteria = new AgentAutoCompleteCriteria
            {
                AgentId = model.AgentId,
                Text = model.UserName
            };

            var agents = _customerService.GetAgentAutoComplete(criteria);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = agents.Select(m => m.UserName).ToArray()
            });
        }

        /// <summary>
        /// Get member auto complete.
        /// </summary>
        /// <param name="model">Customer Auto complete view model.</param>
        /// <returns>Action result.</returns>
        [NonAction]
        public JsonResult SearchMemberAutoComplete(CustomerSearchAutoCompleteViewModel model)
        {
            var criteria = new MemberAutoCompleteCriteria
            {
                AgentId = model.AgentId,
                Text = model.UserName
            };

            var members = _customerService.GetMemberAutoComplete(criteria);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = members.Select(m => m.UserName).ToArray()
            });
        }

        /// <summary>
        /// Get IP Information.
        /// </summary>
        /// <param name="model">IP view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult GetIpInformation(IpSearchViewModel model)
        {
            var criteria = new GetIpInformationCriteria { Ip = model.IpAddress };
            var ipInformation = _ipService.GetIpInformation(criteria).FirstOrDefault();

            return View(new IpInformationViewModel
            {
                IpAddress = model.IpAddress,
                City = ipInformation.IpCity,
                Country = ipInformation.CountryLong
            });
        }

        /// <summary>
        /// Reset Password.
        /// </summary>
        /// <param name="memberId">Member Id.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult ResetPasswordMember(int memberId)
        {
            if (!_permissionService.IsDownLineMember(memberId))
            {
                return RedirectToAccessDeniedPage();
            }

            return View(
                "~/Views/CustomerList/ResetPassword.cshtml",
                new ResetPasswordViewModel
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
        public ActionResult ResetPasswordMember(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_permissionService.IsDownLineMember(model.CustId))
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

            return View("~/Views/CustomerList/ResetPassword.cshtml");
        }

        /// <summary>
        /// Reset Password.
        /// </summary>
        /// <param name="agentId">Agent Id information.</param>
        /// <param name="agentLevel">Agent Level.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult ResetPasswordAgent(int agentId, int agentLevel)
        {
            if (!_permissionService.IsDownLineAgent(agentId))
            {
                return RedirectToAccessDeniedPage();
            }

            return View(
                "~/Views/CustomerList/ResetPassword.cshtml",
                new ResetPasswordViewModel
                {
                    CustId = agentId,
                    IsLastLevel = CurrentSession.AgentInfo.IsLastAgent(agentLevel)
                });
        }

        /// <summary>
        /// Reset Password Agent.
        /// </summary>
        /// <param name="model">Reset Password View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult ResetPasswordAgent(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_permissionService.IsDownLineAgent(model.CustId))
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

            return View("~/Views/CustomerList/ResetPassword.cshtml");
        }

        /// <summary>
        /// Edit Info Member.
        /// </summary>
        /// <param name="memberId">Member Id.</param>
        /// <param name="username">Username member.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult EditInfoMember(int memberId, string username)
        {
            if (!_permissionService.IsDirectDownLineMember(memberId))
            {
                return RedirectToAccessDeniedPage();
            }

            var memberInfo = new CustomerEntity { CustId = memberId };

            _memberService.GetMemberInformation<CustomerEntity>(
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
        public ActionResult EditInfoMember(EditInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_permissionService.IsDirectDownLineMember(model.CustId))
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
        /// Reset Password.
        /// </summary>
        /// <param name="agentId">Agent Id information.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult EditInfoAgent(int agentId)
        {
            if (!_permissionService.IsDirectDownLineAgent(agentId))
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

            return View(
                "~/Views/CustomerList/EditInfo.cshtml",
                new EditInfoViewModel
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
        public ActionResult EditInfoAgent(EditInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_permissionService.IsDirectDownLineAgent(model.CustId))
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

            return View("~/Views/CustomerList/EditInfo.cshtml", model);
        }
    }
}