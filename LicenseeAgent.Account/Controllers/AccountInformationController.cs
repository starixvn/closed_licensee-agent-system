// ----------------------------------------------------------------------
// <copyright file="AccountInformationController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Account.Core;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Account.ViewModels;
    using LicenseeAgent.Account.ViewResults;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccountInformationController : BaseController
    {
        private readonly ISearchUserService _searchUserService;

        public AccountInformationController(ISearchUserService searchUserService)
        {
            _searchUserService = searchUserService;
        }

        /// <summary>
        /// Index page search user.
        /// </summary>
        /// <returns>View action result.</returns>
        [HttpGet]
        [AppAuthorize(OnlyAuthorize = true)]
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
            if (ModelState.IsValid)
            {
                InvalidModelViewResult();
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
        public ActionResult Suggest(SuggestionViewModel model)
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
                MsgId = Result.SuccessValue,
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
                MsgId = Result.SuccessValue,
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
                UserId = CurrentSession.AdminInfo.UserId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName
            };

            var users = _searchUserService.SuggestAdmin(criteria);

            return Json(new
            {
                MsgId = Result.SuccessValue,
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

            var entities = _searchUserService.GetMembersAgent<CustomerEntity, UplineEntity, DownlineEntity>(criteria);

            var customer = (entities[typeof(CustomerEntity).Name] as List<CustomerEntity>).FirstOrDefault();
            var uplines = entities[typeof(UplineEntity).Name] as List<UplineEntity>;
            var downlines = entities[typeof(DownlineEntity).Name] as List<DownlineEntity>;

            if (criteria.MsgId != 0)
            {
                ViewBag.Result = new SearchUserViewResult
                {
                    MsgId = criteria.MsgId,
                    MsgContent = GetErrorMsg(criteria.MsgId)
                };
            }
            else
            {
                ViewBag.PageContext = new PageContext(customer.CustId, customer.Level, customer.NumberOfLevel)
                {
                    IsDirectDownline = new SessionWrapper().IsDirectDownline(CurrentSession.AgentInfo.AgentLevel),
                    IsLastLevel = ModelHelper.IsLastAgent(customer.Level, customer.NumberOfLevel),
                    IsMember = customer.Level == Constants.MemberLevel
                };
            }

            return View(new SearchUserResultViewModel
            {
                Customer = customer,
                Uplines = uplines,
                Downlines = downlines,
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
                UserId = CurrentSession.AdminInfo.UserId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName,
                PageSize = Constants.PageSize
            };

            var entities = _searchUserService.GetMembersAdmin<CustomerEntity, UplineEntity, DownlineEntity>(criteria);

            var customer = (entities[typeof(CustomerEntity).Name] as List<CustomerEntity>).FirstOrDefault();
            var uplines = entities[typeof(UplineEntity).Name] as List<UplineEntity>;
            var downlines = entities[typeof(DownlineEntity).Name] as List<DownlineEntity>;

            if (criteria.MsgId != 0)
            {
                var result = new SearchUserViewResult
                {
                    MsgId = criteria.MsgId,
                    MsgContent = GetErrorMsg(criteria.MsgId),
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
                    IsLastLevel = ModelHelper.IsLastAgent(customer.Level, customer.NumberOfLevel),
                    IsMember = customer.Level == Constants.MemberLevel
                };
            }

            return View(new SearchUserResultViewModel
            {
                Customer = customer,
                Uplines = uplines,
                Downlines = downlines,
                PageIndex = criteria.PageIndex,
                TotalPages = (int)Math.Ceiling((double)criteria.TotalRows / criteria.PageSize)
            });
        }
    }
}