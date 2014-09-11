namespace LicenseeAgent.Home.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Home.Criteria.TopMenu;
    using LicenseeAgent.Home.Services;
    using LicenseeAgent.Home.ViewModels.TopMenu;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// TopMenuController class.
    /// </summary>
    [AppAuthorize(IsValidateAgent = false)]
    [AccountType(AccountType.Admin)]
    public class TopMenuController : BaseController
    {
        #region Variables

        private readonly ITopMenuService _topMenuService;

        #endregion Variables

        #region Constructors

        public TopMenuController()
            : this(null)
        {
        }

        public TopMenuController(ITopMenuService topMenuService)
        {
            _topMenuService = topMenuService ?? new TopMenuService();
        }

        #endregion Constructors

        #region Menu to select licensee & currency

        /// <summary>
        /// Show menu to select licensee and currency and agent name.
        /// </summary>
        /// <returns>Partial view model.</returns>
        [ChildActionOnly]
        public ActionResult SelectMenu()
        {
            if (!CurrentSession.IsViewSelectMenu())
            {
                return Content("Error");
            }

            var model = new SelectMenuViewModel();
            if (CurrentSession.IsViewSelectLicensee())
            {
                model.Licensees = _topMenuService.GetLicensees(CurrentSession.LoginUserId);
                if (CurrentSession.AgentInfo != null)
                {
                    model.LicenseeId = CurrentSession.AgentInfo.LicenseeId;
                    model.LicenseeName = model.Licensees.Where(x => x.LicenseeId == model.LicenseeId).Select(x => x.LicenseeName).FirstOrDefault();
                    model.CurrencyId = CurrentSession.AgentInfo.CurrencyId;
                    model.Currencies = _topMenuService.GetLicenseeCurrencies(new GetCurrencyOfLicenseeCriteria
                    {
                        UserId = CurrentSession.LoginUserId,
                        LicenseeId = model.LicenseeId
                    });
                    model.CurrencyName = model.Currencies.Where(x => x.Currency == model.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault();
                }
            }
            else
            {
                model.LicenseeId = CurrentSession.OnLicenseeId;
                model.Currencies = _topMenuService.GetLicenseeCurrencies(new GetCurrencyOfLicenseeCriteria
                {
                    UserId = CurrentSession.LoginUserId,
                    LicenseeId = model.LicenseeId
                });
                if (CurrentSession.AgentInfo != null)
                {
                    model.CurrencyId = CurrentSession.AgentInfo.CurrencyId;
                    model.CurrencyName = model.Currencies.Where(x => x.Currency == model.CurrencyId).Select(x => x.CurrencyName).FirstOrDefault();
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// View data list currency.
        /// </summary>
        /// <param name="model">Get currency licensee criteria.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        public JsonResult GetCurrencies(CurrencyLicenseeViewModel model)
        {
            var criteria = new GetCurrencyOfLicenseeCriteria
            {
                UserId = CurrentSession.LoginUserId,
                LicenseeId = model.LicenseeId
            };

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = _topMenuService.GetLicenseeCurrencies(criteria)
            });
        }

        #endregion Menu to select licensee & currency
    }
}