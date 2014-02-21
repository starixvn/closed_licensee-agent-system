// ----------------------------------------------------------------------
// <copyright file="AccountInformationController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.AccountInformation.Criteria;
    using LicenseeAgent.AccountInformation.Entities;
    using LicenseeAgent.AccountInformation.Services;
    using LicenseeAgent.AccountInformation.ViewModels;
    using LicenseeAgent.AccountInformation.ViewResults;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccountInformationController : BaseController
    {
        private readonly ISearchUserService _searchUserService;
        private readonly IMemberService _memberService;

        public AccountInformationController(
            ISearchUserService searchUserService,
            IMemberService memberService,
            IDownlLinePermissionService permissionService)
            : base(permissionService)
        {
            _searchUserService = searchUserService;
            _memberService = memberService;
        }

        /// <summary>
        /// Index page search user.
        /// </summary>
        /// <returns>View action result.</returns>
        [HttpGet]
        [AppAuthorize(IsValidateAgent = false)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Index page search user.
        /// </summary>
        /// <param name="model">Search user view model.</param>
        /// <returns>View action result.</returns>
        [HttpPost]
        public ActionResult Search(SearchUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelViewResult();
            }

            if (CurrentSession.AgentInfo.IsLevel0)
            {
                return SearchAdmin(model);
            }
            else
            {
                return SearchAgent(model);
            }
        }

        /// <summary>
        /// Suggest username.
        /// </summary>
        /// <param name="model">Suggestion view model.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        public JsonResult Suggest(SuggestionViewModel model)
        {
            if (CurrentSession.AgentInfo.IsLevel0)
            {
                return SuggestAdmin(model);
            }
            else
            {
                return SuggestAgent(model);
            }
        }

        /// <summary>
        /// Get down line list.
        /// </summary>
        /// <param name="model">Down line view model.</param>
        /// <returns>List down line.</returns>
        [HttpPost]
        public JsonResult GetDownlines(DownlineViewModel model)
        {
            var criteria = new GetDownlineListCriteria
            {
                AgentId = model.AgentId,
                PageIndex = model.PageIndex,
                PageSize = model.PageSize ?? Constants.PageSize
            };

            var downlines = _searchUserService.GetDownlines(criteria);

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = downlines
            });
        }

        /// <summary>
        /// Suggest username by Agent Login.
        /// </summary>
        /// <param name="model">Suggestion view model.</param>
        /// <returns>JSON result.</returns>
        [NonAction]
        public JsonResult SuggestAgent(SuggestionViewModel model)
        {
            var criteria = new AgentSuggestSearchMemberCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.AgentInfo.AgentId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName
            };

            var users = _searchUserService.SuggestAgent(criteria);

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = users.Select(m => m.UserName).ToArray()
            });
        }

        /// <summary>
        /// Suggest username by Admin Login.
        /// </summary>
        /// <param name="model">Suggestion view model.</param>
        /// <returns>JSON result.</returns>
        [NonAction]
        public JsonResult SuggestAdmin(SuggestionViewModel model)
        {
            var criteria = new AdminSuggestSearchMemberCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.LoginUserId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName
            };

            var users = _searchUserService.SuggestAdmin(criteria);

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = users.Select(m => m.UserName).ToArray()
            });
        }

        /// <summary>
        /// Search user by Agent.
        /// </summary>
        /// <param name="model">Search user view model.</param>
        /// <returns>Action result.</returns>
        [NonAction]
        public ActionResult SearchAgent(SearchUserViewModel model)
        {
            var criteria = new AgentGetMemberListCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.AgentInfo.AgentId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName,
                PageSize = Constants.PageSize
            };

            var entities = _searchUserService.GetMembers<CustomerEntity, UplineEntity, DownlineEntity>(criteria);

            var customer = (entities[typeof(CustomerEntity).Name] as List<CustomerEntity>).FirstOrDefault();
            var uplines = entities[typeof(UplineEntity).Name] as List<UplineEntity>;
            var downlines = entities[typeof(DownlineEntity).Name] as List<DownlineEntity>;

            SetAgentView(criteria, customer);

            return View(new SearchUserResultViewModel
            {
                Customer = GetFullInformation(customer),
                Uplines = uplines,
                Downlines = downlines,
                Balance = customer == null ? null : GetBalance(customer),
                PageIndex = criteria.PageIndex,
                TotalPages = (int)Math.Ceiling((double)criteria.TotalRows / criteria.PageSize)
            });
        }

        /// <summary>
        /// Search user by Admin.
        /// </summary>
        /// <param name="model">Search user view model.</param>
        /// <returns>Action result.</returns>
        [NonAction]
        public ActionResult SearchAdmin(SearchUserViewModel model)
        {
            var criteria = new AdminGetMemberListCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.LoginUserId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName,
                PageSize = Constants.PageSize
            };

            var entities = _searchUserService.GetMembers<CustomerEntity, UplineEntity, DownlineEntity>(criteria);

            var customer = (entities[typeof(CustomerEntity).Name] as List<CustomerEntity>).FirstOrDefault();
            var uplines = entities[typeof(UplineEntity).Name] as List<UplineEntity>;
            var downlines = entities[typeof(DownlineEntity).Name] as List<DownlineEntity>;

            SetAdminView(criteria, customer);

            return View(new SearchUserResultViewModel
            {
                Customer = GetFullInformation(customer),
                Uplines = uplines,
                Downlines = downlines,
                Balance = customer == null ? null : GetBalance(customer),
                PageIndex = criteria.PageIndex,
                TotalPages = (int)Math.Ceiling((double)criteria.TotalRows / criteria.PageSize)
            });
        }

        /// <summary>
        /// Get full information when customer is member.
        /// </summary>
        /// <param name="customer">Customer Entity.</param>
        /// <returns>Customer Entity After merge.</returns>
        [NonAction]
        public CustomerEntity GetFullInformation(CustomerEntity customer)
        {
            if (ViewBag.PageContext != null && (bool)ViewBag.PageContext.IsMember)
            {
                var serviceResult = _memberService.GetMemberInformation<CustomerEntity>(
                                    customer,
                                    new GetMemberInfoAction
                                    {
                                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                                        IBCCustId = customer.CustId
                                    });

                if (serviceResult.ErrCode != 0)
                {
                    ViewBag.Result = new SearchUserViewResult
                    {
                        ErrCode = serviceResult.ErrCode,
                        ErrMsg = serviceResult.ErrMsg
                    };
                }
            }

            return customer;
        }

        /// <summary>
        /// Set Admin view.
        /// </summary>
        /// <param name="criteria">Admin member list criteria.</param>
        /// <param name="customer">Customer entity.</param>
        [NonAction]
        public void SetAdminView(AdminGetMemberListCriteria criteria, CustomerEntity customer)
        {
            if (criteria.MsgId != 0 || customer == null)
            {
                var result = new SearchUserViewResult
                {
                    ErrCode = criteria.MsgId,
                    ErrMsg = GetErrMsg(criteria.MsgId),
                    IsChangeCurrency = criteria.AgentCurrency != 0,
                    CurrencyId = criteria.AgentCurrency,
                    CurrencyName = criteria.AgentCurrencyName
                };

                ViewBag.Result = result;
            }
            else
            {
                ViewBag.PageContext = new PageContext(customer.CustId, customer.Level, customer.NumberOfLevel)
                {
                    IsDirectDownline = true,
                    IsLastLevel = ModelHelper.IsLastAgent(customer.Level, customer.NumberOfLevel),
                    IsMember = customer.Level == Constants.MemberLevel
                };
            }
        }

        /// <summary>
        /// Set agent view.
        /// </summary>
        /// <param name="criteria">Agent member list criteria.</param>
        /// <param name="customer">Customer entity.</param>
        [NonAction]
        public void SetAgentView(AgentGetMemberListCriteria criteria, CustomerEntity customer)
        {
            if (criteria.MsgId != 0 || customer == null)
            {
                ViewBag.Result = new SearchUserViewResult
                {
                    ErrCode = criteria.MsgId,
                    ErrMsg = GetErrMsg(criteria.MsgId)
                };
            }
            else
            {
                var isMember = customer.Level == Constants.MemberLevel;
                var isLastLevel = ModelHelper.IsLastAgent(customer.Level, customer.NumberOfLevel)
                                    || ModelHelper.IsLastAgent(CurrentSession.AgentInfo.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel);
                var isDirectDownline = (isLastLevel && isMember)
                                        || CurrentSession.IsDirectDownline(customer.CustId == CurrentSession.AgentInfo.AgentId ? customer.Level : customer.Level - 1);

                ViewBag.PageContext = new PageContext(customer.CustId, customer.Level, customer.NumberOfLevel)
                {
                    IsDirectDownline = isDirectDownline,
                    IsLastLevel = isLastLevel,
                    IsMember = isMember,
                };
            }
        }

        /// <summary>
        /// Get balance of customer.
        /// </summary>
        /// <param name="customer">Customer information.</param>
        /// <returns>Balance entity.</returns>
        [NonAction]
        public BalanceEntity GetBalance(CustomerEntity customer)
        {
            if (customer.Level == Constants.MemberLevel)
            {
                return _searchUserService.GetMemberBalance(new MemberBalanceCriteria { CustId = customer.CustId });
            }
            else
            {
                return _searchUserService.GetAgentBalance(new AgentBalanceCriteria { AgentId = customer.CustId });
            }
        }
    }
}