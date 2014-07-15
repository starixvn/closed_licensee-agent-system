namespace LicenseeAgent.AccountInformation.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.AccountInformation.Criteria;
    using LicenseeAgent.AccountInformation.Entities;
    using LicenseeAgent.AccountInformation.Services;
    using LicenseeAgent.AccountInformation.ViewModels;
    using LicenseeAgent.AccountInformation.ViewResults;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// AccountInformationController class.
    /// </summary>
    public class AccountInformationController : BaseController
    {
        private readonly ISearchUserService _searchUserService;
        private readonly IMemberService _memberService;
        private readonly ICoreService _coreService;

        public AccountInformationController()
            : this(null, null, null)
        {
        }

        public AccountInformationController(
            ISearchUserService searchUserService,
            IMemberService memberService,
            ICoreService coreService)
        {
            _searchUserService = searchUserService ?? new SearchUserService();
            _memberService = memberService ?? new MemberService();
            _coreService = coreService ?? new CoreService();
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

            return SearchAgent(model);
        }

        /// <summary>
        /// Suggest username.
        /// </summary>
        /// <param name="model">Suggestion view model.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        public JsonResult Suggest(SuggestionViewModel model)
        {
            return CurrentSession.AgentInfo.IsLevel0
                ? SuggestAdmin(model)
                : SuggestAgent(model);
        }

        /// <summary>
        /// Get down line list.
        /// </summary>
        /// <param name="model">Down line view model.</param>
        /// <returns>List down line.</returns>
        [HttpPost]
        public JsonResult GetDownlines(DownLineViewModel model)
        {
            var criteria = new GetDirectDownLineCriteria
            {
                AgentId = model.AgentId,
                PageIndex = model.PageIndex,
                PageSize = model.PageSize ?? Constants.PageSize
            };

            var downlines = _searchUserService.GetDirectDownlines(criteria);

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

            var searchMemberEntity = _searchUserService.GetMembers(criteria);
            SetAgentView(criteria, searchMemberEntity.Customer);

            return View(new SearchUserResultViewModel
            {
                Customer = GetFullInformation(searchMemberEntity.Customer),
                Uplines = searchMemberEntity.UpLines,
                Downlines = searchMemberEntity.DownLines,
                Balance = searchMemberEntity.Customer == null ? null : GetBalance(searchMemberEntity.Customer),
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

            var searchMemberEntity = _searchUserService.GetMembers(criteria);
            SetAdminView(criteria, searchMemberEntity.Customer);

            return View(new SearchUserResultViewModel
            {
                Customer = GetFullInformation(searchMemberEntity.Customer),
                Uplines = searchMemberEntity.UpLines,
                Downlines = searchMemberEntity.DownLines,
                Balance = searchMemberEntity.Customer == null ? null : GetBalance(searchMemberEntity.Customer),
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
                var serviceResult = _memberService.GetMemberInformation(
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
                var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
                ViewBag.PageContext = new PageContext(CurrentSession, customer.CustId, customer.Level)
                {
                    IsMember = customer.Level == Constants.MemberLevel,
                    IsOfferedVirtualSports = _coreService.HasProductOffered(productInfos, (int)Product.Product_5)
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
                var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
                ViewBag.PageContext = new PageContext(CurrentSession, customer.CustId, customer.Level)
                {
                    IsMember = isMember,
                    IsOfferedVirtualSports = _coreService.HasProductOffered(productInfos, (int)Product.Product_5)
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
            return customer.Level == Constants.MemberLevel
                ? _searchUserService.GetMemberBalance(new MemberBalanceCriteria { CustId = customer.CustId })
                : _searchUserService.GetAgentBalance(new AgentBalanceCriteria { AgentId = customer.CustId });
        }
    }
}