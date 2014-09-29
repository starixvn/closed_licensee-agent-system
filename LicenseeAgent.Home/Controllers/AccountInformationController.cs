namespace LicenseeAgent.Home.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Home.Criteria.AccountInformation;
    using LicenseeAgent.Home.Entities.AccountInformation;
    using LicenseeAgent.Home.Services;
    using LicenseeAgent.Home.ViewModels.AccountInformation;
    using LicenseeAgent.Home.ViewResults.AccountInformation;
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
        #region Variables

        private readonly IAccountInformationService _accountInformationService;
        private readonly IMemberService _memberService;
        private readonly ICoreService _coreService;

        #endregion Variables

        #region Constructors

        public AccountInformationController()
            : this(null, null, null)
        {
        }

        public AccountInformationController(
            IAccountInformationService accountInformationService,
            IMemberService memberService,
            ICoreService coreService)
        {
            _accountInformationService = accountInformationService ?? new AccountInformationService();
            _memberService = memberService ?? new MemberService();
            _coreService = coreService ?? new CoreService();
        }

        #endregion Constructors

        #region Index page

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

        #endregion Index page

        #region User suggestion

        /// <summary>
        /// Suggest username.
        /// </summary>
        /// <param name="model">Suggestion view model.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        public JsonResult Suggest(SuggestionViewModel model)
        {
            return CurrentSession.AgentInfo.IsLevel0
                ? SuggestByAdmin(model)
                : SuggestByAgent(model);
        }

        /// <summary>
        /// Suggest username by Agent Login.
        /// </summary>
        /// <param name="model">Suggestion view model.</param>
        /// <returns>JSON result.</returns>
        [NonAction]
        public JsonResult SuggestByAgent(SuggestionViewModel model)
        {
            var criteria = new AgentSuggestSearchMemberCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.AgentInfo.AgentId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName,
                LegendType = (short)CurrentSession.AgentInfo.LegendType
            };

            var users = _accountInformationService.SuggestByAgent(criteria);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = users.Select(m => m.UserName).ToList()
            });
        }

        /// <summary>
        /// Suggest username by Admin Login.
        /// </summary>
        /// <param name="model">Suggestion view model.</param>
        /// <returns>JSON result.</returns>
        [NonAction]
        public JsonResult SuggestByAdmin(SuggestionViewModel model)
        {
            var criteria = new AdminSuggestSearchMemberCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.LoginUserId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName,
                LegendType = (short)CurrentSession.AgentInfo.LegendType
            };

            var users = _accountInformationService.SuggestByAdmin(criteria);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = users.Select(m => m.UserName).ToList()
            });
        }

        #endregion User suggestion

        #region Search user

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

            return CurrentSession.AgentInfo.IsLevel0
                ? SearchByAdmin(model)
                : SearchByAgent(model);
        }

        /// <summary>
        /// Search user by Agent.
        /// </summary>
        /// <param name="model">Search user view model.</param>
        /// <returns>Action result.</returns>
        [NonAction]
        public ActionResult SearchByAgent(SearchUserViewModel model)
        {
            var criteria = new AgentGetMemberListCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.AgentInfo.AgentId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName,
                PageSize = Constants.PageSize,
                LegendType = (short)CurrentSession.AgentInfo.LegendType
            };

            var searchMemberEntity = _accountInformationService.GetMembers(criteria);
            SetAgentView(criteria, searchMemberEntity.Customer);
            var isShowStatement = searchMemberEntity.Customer == null
                ? false
                : !(CurrentSession.AgentInfo.LegendType == LegendType.Deposit && searchMemberEntity.Customer.Level != Constants.MemberLevel);
            return View(new SearchUserResultViewModel
            {
                Customer = GetFullInformation(searchMemberEntity.Customer),
                Uplines = searchMemberEntity.UpLines,
                Downlines = searchMemberEntity.DownLines,
                Balance = GetBalance(searchMemberEntity.Customer),
                ProductSettings = GetProductSettings(searchMemberEntity.Customer),
                PageIndex = criteria.PageIndex,
                TotalPages = (int)Math.Ceiling((double)criteria.TotalRows / criteria.PageSize),
                IsShowStatement = isShowStatement
            });
        }

        /// <summary>
        /// Set agent view.
        /// </summary>
        /// <param name="criteria">Agent member list criteria.</param>
        /// <param name="customer">Customer entity.</param>
        [NonAction]
        public void SetAgentView(AgentGetMemberListCriteria criteria, CustomerEntity customer)
        {
            if (criteria.ErrCode != Result.SuccessValue || customer == null)
            {
                ViewBag.Result = new SearchUserViewResult
                {
                    ErrCode = criteria.ErrCode,
                    ErrMsg = GetErrMsg(criteria.ErrCode)
                };
            }
            else
            {
                var isMember = customer.Level == Constants.MemberLevel;
                var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
                ViewBag.PageContext = new PageContext(CurrentSession, isMember, customer.CustId, customer.Level, customer.UserName)
                {
                    IsOfferedVirtualSports = _coreService.HasProductOffered(productInfos, (int)ProductCode.Product_5)
                };
            }
        }

        /// <summary>
        /// Search user by Admin.
        /// </summary>
        /// <param name="model">Search user view model.</param>
        /// <returns>Action result.</returns>
        [NonAction]
        public ActionResult SearchByAdmin(SearchUserViewModel model)
        {
            var criteria = new AdminGetMemberListCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.LoginUserId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName,
                LegendType = (short)CurrentSession.AgentInfo.LegendType,
                PageSize = Constants.PageSize
            };

            var searchMemberEntity = _accountInformationService.GetMembers(criteria);
            SetAdminView(criteria, searchMemberEntity.Customer);
            var isShowStatement = searchMemberEntity.Customer == null
                ? false
                : !(CurrentSession.AgentInfo.LegendType == LegendType.Deposit && searchMemberEntity.Customer.Level != Constants.MemberLevel);
            return View(new SearchUserResultViewModel
            {
                Customer = GetFullInformation(searchMemberEntity.Customer),
                Uplines = searchMemberEntity.UpLines,
                Downlines = searchMemberEntity.DownLines,
                Balance = GetBalance(searchMemberEntity.Customer),
                ProductSettings = GetProductSettings(searchMemberEntity.Customer),
                PageIndex = criteria.PageIndex,
                TotalPages = (int)Math.Ceiling((double)criteria.TotalRows / criteria.PageSize),
                IsShowStatement = isShowStatement
            });
        }

        /// Set Admin view.
        /// </summary>
        /// <param name="criteria">Admin member list criteria.</param>
        /// <param name="customer">Customer entity.</param>
        [NonAction]
        public void SetAdminView(AdminGetMemberListCriteria criteria, CustomerEntity customer)
        {
            if (criteria.ErrCode != Result.SuccessValue || customer == null)
            {
                var result = new SearchUserViewResult
                {
                    ErrCode = criteria.ErrCode,
                    ErrMsg = GetErrMsg(criteria.ErrCode),
                    IsChangeCurrency = criteria.AgentCurrency != 0,
                    CurrencyId = criteria.AgentCurrency,
                    CurrencyName = criteria.AgentCurrencyName
                };

                ViewBag.Result = result;
            }
            else
            {
                var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
                var isMember = customer.Level == Constants.MemberLevel;
                ViewBag.PageContext = new PageContext(CurrentSession, isMember, customer.CustId, customer.Level, customer.UserName)
                {
                    IsOfferedVirtualSports = _coreService.HasProductOffered(productInfos, (int)ProductCode.Product_5)
                };
            }
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

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    ViewBag.Result = new SearchUserViewResult
                    {
                        ErrCode = serviceResult.ErrCode,
                        ErrMsg = serviceResult.ErrMsg
                    };
                }

                if (customer != null && CurrentSession.AgentInfo.IsDeposit)
                {
                    customer.FirstName = string.Empty;
                    customer.LastName = string.Empty;
                    customer.Phone = string.Empty;
                    customer.Mobile = string.Empty;
                    customer.Fax = string.Empty;
                }
            }

            return customer;
        }

        /// <summary>
        /// Gets the product settings.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns list of product setting.</returns>
        [NonAction]
        public IList<ProductSettingEntity> GetProductSettings(CustomerEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            if (entity.Level != Constants.MemberLevel)
            {
                return _accountInformationService.GetAgentProductSetting(new GetAgentProductSettingCriteria
                {
                    AgentId = entity.CustId,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                });
            }
            else
            {
                return _accountInformationService.GetMemberProductSetting(new GetMemberProductSettingCriteria
                {
                    CustId = entity.CustId,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                });
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
            return customer == null
                ? null
                : CurrentSession.AgentInfo.IsDeposit
                    ? GetCashBalance(customer)
                    : GetCreditBalance(customer);
        }

        /// <summary>
        /// Gets the cash balance.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Returns balance information.</returns>
        [NonAction]
        public BalanceEntity GetCashBalance(CustomerEntity customer)
        {
            var balanceUp = _coreService.BalanceUpDay;
            return customer.Level == Constants.MemberLevel
                ? _accountInformationService.GetMemberCashBalance(new MemberCashBalanceCriteria
                {
                    CustId = customer.CustId
                })
                : _accountInformationService.GetAgentCashBalance(new AgentCashBalanceCriteria
                {
                    AgentId = customer.CustId,
                    MoveBetDay = balanceUp.MoveBetsDay,
                    BalanceUpDay = balanceUp.BalanceUpDay,
                    Today = DateTime.Now
                });
        }

        /// <summary>
        /// Gets the credit balance.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Returns balance information.</returns>
        [NonAction]
        public BalanceEntity GetCreditBalance(CustomerEntity customer)
        {
            return customer.Level == Constants.MemberLevel
                ? _accountInformationService.GetMemberCreditBalance(new MemberCreditBalanceCriteria
                {
                    CustId = customer.CustId
                })
                : _accountInformationService.GetAgentCreditBalance(new AgentCreditBalanceCriteria
                {
                    AgentId = customer.CustId
                });
        }

        #endregion Search user

        #region Get DownLines

        /// <summary>
        /// Get down line list.
        /// </summary>
        /// <param name="model">Down line view model.</param>
        /// <returns>List down line.</returns>
        [HttpPost]
        public JsonResult GetDownLines(DownLineViewModel model)
        {
            var criteria = new GetDirectDownLineCriteria
            {
                AgentId = model.AgentId,
                PageIndex = model.PageIndex,
                PageSize = model.PageSize ?? Constants.PageSize
            };

            var downlines = _accountInformationService.GetDirectDownlines(criteria);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = downlines
            });
        }

        #endregion Get DownLines
    }
}