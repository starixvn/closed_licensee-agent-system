namespace LicenseeAgent.Customer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Customer.Criteria;
    using LicenseeAgent.Customer.Entities;
    using LicenseeAgent.Customer.Services;
    using LicenseeAgent.Customer.ViewModels;
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
    /// CustomerListController class.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Review")]
    public class CustomerListController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IIpInformationService _ipInformationService;
        private readonly IMemberService _memberService;
        private readonly IDownLinePermissionService _downLinePermissionService;
        private readonly ICoreService _coreService;

        public CustomerListController()
            : this(null, null, null, null, null)
        {
        }

        public CustomerListController(
            ICustomerService customerService,
            IIpInformationService ipInformationService,
            IMemberService memberService,
            IDownLinePermissionService downLinePermissionService,
            ICoreService coreService)
        {
            _customerService = customerService ?? new CustomerService();
            _ipInformationService = ipInformationService ?? new IpInformationService();
            _memberService = memberService ?? new MemberService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _coreService = coreService ?? new CoreService();
        }

        #region Member List

        /// <summary>
        /// Members the list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Page member list.</returns>
        [HttpGet]
        public ActionResult MemberList(AgentInfoViewModel model)
        {
            var hasAgentModel = model.AgentId != 0;
            var breadModel = new AgentBreadcrumbViewModel(
                hasAgentModel ? model.AgentId : CurrentSession.AgentInfo.AgentId,
                hasAgentModel ? model.AgentName : CurrentSession.AgentInfo.UserName,
                hasAgentModel ? model.AgentLevel : CurrentSession.AgentInfo.AgentLevel);

            var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            ViewBag.PageContext = new PageContext(CurrentSession, breadModel.AgentId, breadModel.AgentLevel)
            {
                IsMember = true,
                IsOfferedVirtualSports = _coreService.HasProductOffered(productInfos, (int)ProductCode.Product_5)
            };

            return View("~/Views/CustomerList/Index.cshtml", breadModel);
        }

        #endregion Member List

        /// <summary>
        /// Agent List.
        /// </summary>
        /// <param name="model">Agent information view model.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult Index(AgentInfoViewModel model)
        {
            var hasAgentModel = model.AgentId != 0;
            
            var breadModel = new AgentBreadcrumbViewModel(
                hasAgentModel ? model.AgentId : CurrentSession.AgentInfo.AgentId,
                hasAgentModel ? model.AgentName : CurrentSession.AgentInfo.UserName,
                hasAgentModel ? model.AgentLevel : CurrentSession.AgentInfo.AgentLevel,
                hasAgentModel ? model.IsMember : false);

            var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            ViewBag.PageContext = new PageContext(CurrentSession, breadModel.AgentId, breadModel.AgentLevel)
            {
                IsOfferedVirtualSports = _coreService.HasProductOffered(productInfos, (int)ProductCode.Product_5),
                IsMember = model.IsMember
            };

            return View(breadModel);
        }

        /// <summary>
        /// Agents the list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult Index([ModelBinder(typeof(AgentBreadcrumbModelBinder))]AgentBreadcrumbViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
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

            var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            ViewBag.PageContext = new PageContext(CurrentSession, model.AgentId, model.AgentLevel)
            {
                IsOfferedVirtualSports = _coreService.HasProductOffered(productInfos, (int)ProductCode.Product_5),
                IsMember = model.IsMember
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
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var isGetAll = string.IsNullOrEmpty(model.UserName) && model.Status == null;
            if (model.IsMember)
            {
                /*var list = new List<MemberEntity>{
                    new MemberEntity{
                        AgentId = 20,
                        AvailableFund = 10,
                        CreatedDate = DateTime.Now,
                        CustId = 123,
                        FirstName = "x",
                        IpLogin = "aa",
                        Status = 0,
                        SystemId = "aaa",
                        UserName = "Test Data"
                    }
                };
                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    List = list,
                    CurrentPage = model.PageIndex,
                    ItemsPerPage = model.PageSize,
                    TotalItem = 0
                });*/
                return SearchMembers(model);
            }

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
                AgentId = model.AgentId,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex
            };

            var members = _customerService.GetMemberList(criteria);
            if (criteria.MsgId == Result.SuccessValue && members.Any())
            {
                var serviceResult = _memberService.GetMemberInformations(
                                    members,
                                    new GetMemberInfoListAction
                                    {
                                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                                        LASAgentId = model.AgentId,
                                        IBCCustIdList = string.Join(",", members.Select(a => a.CustId.ToString()).ToArray())
                                    });

                if (!CurrentSession.IsAdmin)
                {
                    members.ToList().ForEach(r => r.SystemId = string.Empty);
                }

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    return JsonResult(serviceResult);
                }
            }

            return Json(new
            {
                ErrCode = criteria.MsgId,
                ErrMsg = GetErrMsg(criteria.MsgId),
                List = members,
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
        public JsonResult SearchMembers(CustomerListViewModel model)
        {
            var serviceResult = GetMemberInformations(model);
            if (serviceResult.ErrCode != Result.SuccessValue)
            {
                return JsonResult(serviceResult);
            }

            var serviceMembers = new List<ServiceMemberEntity>();
            if (serviceResult.Members != null && serviceResult.Members.Any())
            {
                serviceMembers = serviceResult.Members.Where(m => model.Status == null || m.Status == model.Status)
                    .OrderBy(m => m.Username)
                    .Skip(model.PageIndex * model.PageSize)
                    .Take(model.PageSize).ToList();
            }

            if (!serviceMembers.Any())
            {
                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    List = new List<MemberEntity>(),
                    CurrentPage = model.PageIndex,
                    ItemsPerPage = model.PageSize,
                    TotalItem = 0
                });
            }

            var members = _customerService.SearchMembersByIds(new SearchMemberByIdsCriteria
            {
                AgentId = model.AgentId,
                ListCustId = string.Join(",", serviceMembers.Select(m => m.IBCCustId.ToString()).ToArray())
            });
            ServiceMapper.MapObjects(serviceMembers, members, new MemberFilter());

            if (!CurrentSession.IsAdmin)
            {
                members.ToList().ForEach(r => r.SystemId = string.Empty);
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = members,
                CurrentPage = model.PageIndex,
                ItemsPerPage = model.PageSize,
                TotalItem = members.Count()
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
            var ipInformation = _ipInformationService.GetIpInformation(criteria).FirstOrDefault();

            return View(new IpInformationViewModel
            {
                IpAddress = model.IpAddress,
                City = ipInformation.IpCity,
                Country = ipInformation.CountryLong
            });
        }

        [HttpGet]
        [LegendType(LegendType.Deposit)]
        public ViewResult PendingMemberList()
        {
            var model = new PendingMemberListViewModel
            {
                IsLastLevel = CurrentSession.AgentInfo.IsLastLevel
            };

            return View(model);
        }

        /// <summary>
        /// Get pending member list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns result.</returns>
        [HttpPost]
        [LegendType(LegendType.Deposit)]
        public JsonResult PendingMemberList(PendingMemberListViewModel model)
        {
            var agentId = CurrentSession.AgentInfo.AgentId;
            if (!CurrentSession.AgentInfo.IsLastLevel)
            {
                agentId = _customerService.GetAgentIdByUserName(new GetAgentIdByUsernameCriteria
                {
                    UserName = model.DirectAgentUserName,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    LicenseeCurrencyId = CurrentSession.AgentInfo.CurrencyId
                });

                if (agentId == 0)
                {
                    return JsonResult(ErrCode.Msg_2002);
                }
            }

            var result = _memberService.GetDepositMemberInfoList(new GetDepositMemberInfoListAction
            {
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                LASAgentId = agentId,
                DepositOption = (int)DepositOption.DepositNot,
                Username = model.MemberUserName
            });

            return Json(new
            {
                ErrCode = result.ErrCode,
                ErrMsg = result.ErrMsg,
                Data = result.Data
            });
        }

        /// <summary>
        /// Get member information.
        /// </summary>
        /// <param name="model">Customer list view model.</param>
        /// <returns>Member list result.</returns>
        [NonAction]
        public MemberListResult GetMemberInformations(CustomerListViewModel model)
        {
            var action = CurrentSession.AgentInfo.IsCredit
                ? (BaseAction)new GetMemberInfoListAction
                {
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    LASAgentId = model.AgentId
                }
                : (BaseAction)new GetDepositMemberInfoListAction
                {
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    LASAgentId = model.AgentId,
                    DepositOption = (int)DepositOption.DepositOnly,
                    Username = model.UserName
                };

            return _memberService.GetPlainMembers(action);
        }

        /// <summary>
        /// Get agent auto complete.
        /// </summary>
        /// <param name="model">Customer auto complete view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public JsonResult CustomerSearchAutoComplete(CustomerSearchAutoCompleteViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var isLastLevel = CurrentSession.AgentInfo.IsLastAgent(model.AgentLevel);
            return isLastLevel ? SearchMemberAutoComplete(model) : SearchAgentAutoComplete(model);
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
    }
}