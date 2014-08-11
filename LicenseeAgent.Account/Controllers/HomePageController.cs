namespace LicenseeAgent.Account.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using LicenseeAgent.Account.Criteria.HomePage;
    using LicenseeAgent.Account.Entities.HomePage;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Account.ViewModels.HomePage;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// HomeController class.
    /// </summary>
    public class HomePageController : BaseController
    {
        private readonly IHomePageService _homePageService;

        private readonly ICoreService _coreService;

        public HomePageController()
            : this(null, null)
        {
        }

        public HomePageController(
            IHomePageService homePageService,
            ICoreService coreService)
        {
            _homePageService = homePageService ?? new HomePageService();
            _coreService = coreService ?? new CoreService();
        }

        /// <summary>
        /// Home page.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ViewResult Index()
        {
            var agentInfo = CurrentSession.AgentInfo;
            if (agentInfo == null || agentInfo.LegendType == LegendType.Credit)
            {
                return IndexCredit();
            }
            else
            {
                return IndexCash();
            }
        }

        /// <summary>
        /// Home page for legend credit.
        /// </summary>
        /// <returns>Home page view.</returns>
        [NonAction]
        public ViewResult IndexCredit()
        {
            var model = new HomePageViewModel
            {
                StatisticInfo = new StatisticInfo(),
                BalanceInfo = new BalanceInfo()
            };

            if (CurrentSession.AgentInfo != null)
            {
                var statisticInfo = _homePageService.GetStatisticInfo(new GetStatisticCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId
                });

                var balanceUp = _coreService.BalanceUpDay;
                var balanceInfo = _homePageService.GetBalanceInfo(new GetBalanceInfoCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    MoveBetDay = balanceUp.MoveBetsDay
                });

                model = new HomePageViewModel
                {
                    StatisticInfo = statisticInfo,
                    BalanceInfo = balanceInfo
                };
            }

            return View("~/Areas/Account/Views/HomePage/Credit/Index.cshtml", model);
        }

        /// <summary>
        /// Home page for legend cash.
        /// </summary>
        /// <returns>Home page view.</returns>
        [NonAction]
        public ViewResult IndexCash()
        {
            var statisticInfo = _homePageService.GetStatisticInfo(new GetStatisticCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId
            });

            var balanceUp = _coreService.BalanceUpDay;
            var balanceInfo = _homePageService.GetBalanceCashInfo(new GetBalanceCashInfoCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                MoveBetDay = balanceUp.MoveBetsDay,
                BalanceUpDay = balanceUp.BalanceUpDay
            });

            IList<string> memberSiteLinks = new List<string>();
            if (CurrentSession.AgentInfo.IsLastLevel)
            {
                memberSiteLinks = _homePageService.GetMemberSiteLinks(new GetMemberSiteLinkCriteria
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    AgentUserName = CurrentSession.AgentInfo.UserName
                });
            }

            var model = new HomePageCashViewModel
            {
                StatisticInfo = statisticInfo,
                BalanceInfo = balanceInfo,
                MemberSiteLinks = memberSiteLinks
            };

            return View("~/Areas/Account/Views/HomePage/Cash/Index.cshtml", model);
        }

        /// <summary>
        /// Statistics top 10 winning members.
        /// </summary>
        /// <returns>View result.</returns>
        public ViewResult TopWinning()
        {
            var balanceUp = _coreService.BalanceUpDay;
            var criteria = new TopWinningCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                BalanceUpDay = balanceUp.BalanceUpDay,
                MoveBetsDay = balanceUp.MoveBetsDay
            };
            var topWinningData = _homePageService.GetTopWinning(criteria);

            return View(topWinningData);
        }

        /// <summary>
        /// Statistics top 10 turnover members.
        /// </summary>
        /// <returns>View result.</returns>
        public ViewResult TopTurnover()
        {
            var balanceUp = _coreService.BalanceUpDay;
            var criteria = new TopTurnoverCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                BalanceUpDay = balanceUp.BalanceUpDay,
                MoveBetsDay = balanceUp.MoveBetsDay
            };
            var topTurnoverData = _homePageService.GetTopTurnover(criteria);

            return View(topTurnoverData);
        }

        /// <summary>
        /// New members of the month.
        /// </summary>
        /// <returns>View result.</returns>
        public ViewResult NewMembers()
        {
            var newMemberData = _homePageService.GetNewMemberOfMonth(new NewMemberOfMonthCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId
            });

            return View(newMemberData);
        }
    }
}