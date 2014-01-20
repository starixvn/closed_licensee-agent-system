// -----------------------------------------------------------------------
// <copyright file="CustomerController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using LicenseeAgent.Account.Binders;
    using LicenseeAgent.Account.Core;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Account.ViewModels;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _agentService;

        private readonly IIpInformationService _ipService;

        private readonly IMemberService _memberService;

        public CustomerController(ICustomerService agentService, IIpInformationService ipService, IMemberService memberService)
        {
            _agentService = agentService;
            _ipService = ipService;
            _memberService = memberService;
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
                IsDirectDownline = CurrentSession.IsDirectDownline(CurrentSession.AgentInfo.AgentLevel),
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
                AgentId = model.AgentId
            };

            var agents = _agentService.GetAgentList(criteria);
            return Json(new
            {
                ErrCode = criteria.MsgId,
                ErrMsg = GetErrMsg(criteria.MsgId),
                List = agents
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
                AccStatus = model.Status
            };

            var agents = _agentService.SearchAgent(criteria);
            return Json(new
            {
                ErrCode = criteria.MsgId,
                ErrMsg = GetErrMsg(criteria.MsgId),
                List = agents
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

            var members = _agentService.GetMemberList(criteria);

            if (criteria.MsgId == 0)
            {
                var serviceResult = _memberService.GetMemberInformations<MemberEntity>(
                                    members,
                                    new GetMemberInfoListAction
                                    {
                                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                                        LASAgentId = model.AgentId
                                    });

                // if (serviceResult.ErrCode != 0)
                // {
                //    return Json(new
                //    {
                //        ErrCode = serviceResult.ErrCode,
                //        ErrMsg = serviceResult.ErrMsg
                //    });
                // }
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

            var members = _agentService.SearchMember(criteria);

            if (criteria.MsgId == 0)
            {
                var serviceResult = _memberService.GetMemberInformations<MemberEntity>(
                                    members,
                                    new GetMemberInfoListAction
                                    {
                                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                                        LASAgentId = model.AgentId
                                    });

                // if (serviceResult.ErrCode != 0)
                // {
                //    return Json(new
                //    {
                //        ErrCode = serviceResult.ErrCode,
                //        ErrMsg = serviceResult.ErrMsg
                //    });
                // }
            }

            return Json(new
            {
                ErrCode = criteria.MsgId,
                ErrMsg = GetErrMsg(criteria.MsgId),
                List = members.Where(m => m.Status == model.Status)
            });
        }

        /// <summary>
        /// Change agent account status.
        /// </summary>
        /// <param name="model">Agent status view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [DirectDownLineAgent]
        public JsonResult ChangeAgentStatus(AgentStatusViewModel model)
        {
            var criteria = new UpdateAgentAccStatusCriteria
            {
                Updater = CurrentSession.LoginUserId,
                AgentId = model.AgentId,
                AccStatus = model.Status
            };

            _agentService.UpdateAgentAccStatus(criteria);

            if (criteria.MsgId == 0 && model.Status == (byte)AccountStatus.Closed)
            {
                var serviceResult = _memberService.UpdateMemberStatus(new UpdateMemberStatusAction
                {
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    LASAgentId = model.AgentId,
                    IsClosed = true
                });

                if (serviceResult.ErrCode != 0)
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
        [DirectDownLineMember]
        public JsonResult ChangeMemberStatus(MemberStatusViewModel model)
        {
            var serviceResult = _memberService.UpdateMemberStatus(new UpdateMemberStatusAction
            {
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                IBCCustId = model.MemberId,
                IsClosed = model.Status == (byte)AccountStatus.Closed,
                LASAgentId = model.AgentId
            });

            return Json(new
            {
                ErrCode = serviceResult.ErrCode,
                ErrMsg = serviceResult.ErrMsg
            });
        }

        /// <summary>
        /// Get agent auto complete.
        /// </summary>
        /// <param name="model">Customer auto complete view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public JsonResult CustomerSearchAutoComplete(CustomerSearchAutoCompleteViewModel model)
        {
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
                Text = model.Text
            };

            var agents = _agentService.GetAgentAutoComplete(criteria);
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
                Text = model.Text
            };

            var members = _agentService.GetMemberAutoComplete(criteria);
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
            return View(
                "~/Views/Customer/ResetPassword.cshtml",
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
                var logStatement = new ResetPasswordMemberLogCriteria
                {
                    CustId = model.CustId,
                    Doing = string.Format("Url: {0}/ResetPassword/?help=0", AppSettings.TaiwanAPI) +
                            ", SiteName: " + CurrentSession.AgentInfo.LicenseeName +
                            ", IBCCustID:" + model.CustId,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    UserId = CurrentSession.LoginUserId
                };

                // Write log before call API
                _agentService.ResetPasswordMemberLog(logStatement);

                // Call API to update member password
                var serviceResult = _memberService.ResetPassword(new ResetPasswordAction
                {
                    IBCCustId = model.CustId,
                    Password = model.Password,
                    SiteName = CurrentSession.AgentInfo.LicenseeName
                });

                if (serviceResult.ErrCode != 0)
                {
                    ModelState.AddModelError(string.Empty, serviceResult.ErrMsg);
                }
                else
                {
                    ViewBag.Result = Las.UpdateSuccessfully;
                }

                // Write log after call API
                logStatement.ErrCode = serviceResult.ErrCode;
                logStatement.ErrMsg = serviceResult.ErrMsg;
                _agentService.ResetPasswordMemberLog(logStatement);
            }

            return View("~/Views/Customer/ResetPassword.cshtml");
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
            return View(
                "~/Views/Customer/ResetPassword.cshtml",
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
                var criteria = new ResetPasswordCriteria
                {
                    AgentId = model.CustId,
                    Password = model.Password,
                    UserId = CurrentSession.LoginUserId,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    UserSalt = Cryptography.CreateSalt()
                };

                _agentService.ResetPassword(criteria);

                if (criteria.ErrCode != 0)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ViewBag.Result = Las.UpdateSuccessfully;
                }
            }

            return View("~/Views/Customer/ResetPassword.cshtml");
        }
    }
}