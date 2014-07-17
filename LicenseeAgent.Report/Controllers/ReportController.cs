namespace LicenseeAgent.Report.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Core.Entities;
    using Core.Services;
    using Criteria;
    using Fanex.Result.Core;
    using Fanex.Result.Core.DTO;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.BetList.ViewModels;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Services;
    using Services;
    using Utilities;
    using ViewModels;
    using Web.Controllers;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public partial class ReportController : BaseController
    {
        #region Services

        private readonly IDownLinePermissionService _downLinePermissionService;

        private readonly IMemberService _memberService;

        private readonly ICoreService _coreService;

        private readonly IReportService _reportService;

        private readonly IBetListService _betlistService;

        #endregion Services

        #region Constant

        private bool _includeTestData = false;

        #endregion Constant

        #region Constructor

        public ReportController()
            : this(null, null, null, null, null)
        {
        }

        public ReportController(
            ICoreService coreService,
            IReportService reportService,
            IBetListService betlistService,
            IDownLinePermissionService downLinePermissionService,
            IMemberService memberService)
        {
            _coreService = coreService ?? new CoreService();
            _reportService = reportService ?? new ReportService();
            _betlistService = betlistService ?? new BetListService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _memberService = memberService ?? new MemberService();
        }

        #endregion Constructor

        #region Results Report

        /// <summary>
        /// Index page of Results report.
        /// </summary>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Results()
        {
            return View();
        }

        /// <summary>
        /// Index page of Results report.
        /// </summary>
        /// <param name="criteria">Sport criteria.</param>
        /// <returns>Return a view.</returns>
        [HttpPost]
        public JsonResult Results(SportCriteria criteria)
        {
            criteria.IsShowAll = _includeTestData;
            var sports = _reportService.GetSports(criteria);
            var model = new ResultsReportViewModel();
            model.Sports = sports.Select(i => new SelectListItem
            {
                Text = DataRefWrapper.GetSportName(i.SportType),
                Value = i.SportType.ToString()
            }).ToList();

            if (model.Sports.Count > 0)
            {
                model.Sports[0].Selected = true;
                var leagueCriteria = new LeagueCriteria
                {
                    IsShowAll = criteria.IsShowAll,
                    EventDate = criteria.EventDate,
                    SportType = int.Parse(model.Sports[0].Value)
                };

                var leagues = _reportService.GetLeagues(leagueCriteria);
                model.Leagues = leagues.Select(i => new SelectListItem
                {
                    Text = DataRefWrapper.GetLeagueName(i.LeagueId),
                    Value = i.LeagueId.ToString()
                }).OrderBy(j => j.Text).ToList();

                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    Sports = model.Sports,
                    Leagues = model.Leagues,
                    Results = GetMatchResultsData(leagueCriteria, 0)
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Sports = new List<SelectListItem>(),
                Leagues = new List<SelectListItem>(),
                Results = ResultsMatchNoData()
            });
        }

        /// <summary>
        /// Index page of Results report.
        /// </summary>
        /// <param name="criteria">Sport criteria.</param>
        /// <returns>Return a view.</returns>
        [HttpPost]
        public JsonResult OutrightResults(OutrightResultsReportCriteria criteria)
        {
            var setting = new ResultSetting { IsOutRight = true, LanguageCode = LanguageManager.Language };

            var resultsHtmlString = string.Empty;
            var cacheKey = GenerateOutRightCacheKey(criteria);
            var outRightData = (IList<OutRight>)HttpRuntime.Cache.Get(cacheKey) ?? _reportService.GetOutrightResults(criteria);

            if (outRightData.Count > 0)
            {
                var sports = outRightData.GroupBy(i => i.SportType)
                    .Select(j => new SelectListItem
                    {
                        Text = DataRefWrapper.GetSportName(j.First().SportType),
                        Value = j.First().SportType.ToString(),
                        Selected = j.First().SportType == criteria.SportType
                    }).OrderBy(a => int.Parse(a.Value)).ToList();
                var sportType = criteria.SportType == 0 ? int.Parse(sports[0].Value) : criteria.SportType;
                var leagues = outRightData.Where(i => i.SportType == sportType).GroupBy(i => i.LeagueId)
                    .Select(j => new SelectListItem { Text = DataRefWrapper.GetLeagueName(j.First().LeagueId), Value = j.First().LeagueId.ToString() })
                    .OrderBy(k => k.Text).ToList();
                var data = outRightData.Where(i => i.SportType == sportType && (criteria.LeagueId == 0 || criteria.LeagueId == i.LeagueId)).ToList();
                if (data.Count > 0)
                {
                    resultsHtmlString = ResultEngine.HtmlParser(data, setting);
                }

                return Json(new
                {
                    ErrCode = Result.SuccessValue,
                    Sports = sports,
                    Leagues = leagues,
                    Results = resultsHtmlString,
                    TotalRows = data.Count
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Sports = new List<SelectListItem>(),
                Leagues = new List<SelectListItem>(),
                Results = ResultsOutrightNoData(),
                TotalRows = 0
            });
        }

        /// <summary>
        /// Get list of results.
        /// </summary>
        /// <param name="criteria">Results Report criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult GetMatchResults(MatchResultsReportCriteria criteria)
        {
            var results = string.Empty;
            if (criteria.SportType == 0)
            {
                results = ResultsMatchNoData();
            }
            else
            {
                var leagueCriteria = new LeagueCriteria
                {
                    SportType = criteria.SportType,
                    EventDate = criteria.EventDate
                };

                results = GetMatchResultsData(leagueCriteria, criteria.LeagueId);
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Results = results
            });
        }

        /// <summary>
        /// Get list of leagues.
        /// </summary>
        /// <param name="criteria">League criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult GetLeagues(LeagueCriteria criteria)
        {
            var leagues = _reportService.GetLeagues(criteria);
            var leagueItems = leagues.Select(i => new SelectListItem
            {
                Text = DataRefWrapper.GetLeagueName(i.LeagueId),
                Value = i.LeagueId.ToString()
            }).OrderBy(j => j.Text).ToList();

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Leagues = leagueItems,
                Results = GetMatchResultsData(criteria, 0)
            });
        }

        /// <summary>
        /// Get Balance up and move bet dates.
        /// </summary>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult BalanceUpMoveBet()
        {
            var balanceUp = _coreService.BalanceUpDay;
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                BalanceUp = balanceUp.BalanceUpDay.ToShortDateString(),
                MoveBet = balanceUp.MoveBetsDay.ToShortDateString()
            });
        }

        #endregion Results Report

        #region Outstanding Report

        /// <summary>
        /// Outstanding Report.
        /// </summary>
        /// <returns>Return a JSON object.</returns>
        public ActionResult Outstanding()
        {
            ViewBag.LoggedInAgentName = CurrentSession.AgentInfo.UserName;
            ViewBag.LoggedInAgentId = CurrentSession.AgentInfo.AgentId;
            return View();
        }

        /// <summary>
        /// Outstanding Report.
        /// </summary>
        /// <param name="criteria">Outstanding Report Criteria.</param>
        /// <returns>Return a JSON object.</returns>
        [HttpPost]
        public JsonResult Outstanding(OutstandingReportCriteria criteria)
        {
            if (criteria.AgentId != -1 && !_downLinePermissionService.IsDownLineAgent(criteria.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            criteria.AgentId = criteria.AgentId <= 0 ? CurrentSession.AgentInfo.AgentId : criteria.AgentId;
            var agentOutstandings = _reportService.GetOutstandings(criteria);

            decimal sumOutstanding = agentOutstandings.Sum(i => i.Outstanding);
            decimal sumPositiontaking = agentOutstandings.Sum(i => i.Positiontaking);

            return Json(new
            {
                List = agentOutstandings.ToList(),
                SumOutstanding = sumOutstanding,
                SumPositiontaking = sumPositiontaking,
                IsLastLevel = criteria.IsLastLevel,
                ErrCode = Result.SuccessValue
            });
        }

        /// <summary>
        /// Outstanding - Bet List Report.
        /// </summary>
        /// <param name="criteria">Outstanding Report Criteria.</param>
        /// <returns>Return a JSON object.</returns>downlinemember
        [HttpPost]
        public JsonResult OutstandingBetList(OutstandingReportBetListCriteria criteria)
        {
            if (!_downLinePermissionService.IsDownLineMember(criteria.CustId))
            {
                return RedirectToAccessDeniedJson();
            }

            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var betlistTickets = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = betlistTickets,
                IsAdmin = CurrentSession.IsAdmin
            };
            var betListTicketBuilder = new OutstandingBetListTicketBuilder(ticketBuilderParameters);

            var model = new BetListViewModel
            {
                TicketTextList = betListTicketBuilder.BuildTickets(),
                TicketResult = betlistTickets
            };

            return Json(new
            {
                List = model.TicketTextList,
                IsLastLevel = CurrentSession.AgentInfo.IsLastLevel,
                TotalItem = model.TicketTextList.Count(),
                ErrCode = Result.SuccessValue
            });
        }

        #endregion Outstanding Report

        #region Utilities

        /// <summary>
        /// Generate Outright Cache Key.
        /// </summary>
        /// <param name="criteria">Outright Results Report Criteria.</param>
        /// <returns>HTML string.</returns>
        private static string GenerateOutRightCacheKey(OutrightResultsReportCriteria criteria)
        {
            const string PatternKey = "{0}{1}Results.OutRight.Data_{2}";
            return string.Format(PatternKey, criteria.FromDate, criteria.ToDate, CurrentSession.OnUserId);
        }

        /// <summary>
        /// Return match no data.
        /// </summary>
        /// <returns>HTML string.</returns>
        private string ResultsMatchNoData()
        {
            return RenderRazorViewToString("~/Views/Report/ResultsMatch.NoData.cshtml", null);
        }

        /// <summary>
        /// Return outright no data.
        /// </summary>
        /// <returns>HTML string.</returns>
        private string ResultsOutrightNoData()
        {
            return RenderRazorViewToString("~/Views/Report/ResultsOutright.NoData.cshtml", null);
        }

        /// <summary>
        /// Get Match Results Data.
        /// </summary>
        /// <param name="criteria">Match Results Report Criteria.</param>
        /// <param name="leagueId">League ID input.</param>
        /// <returns>HTML string.</returns>
        private string GetMatchResultsData(LeagueCriteria criteria, int leagueId)
        {
            var setting = new ResultSetting
            {
                SportType = (SportType)criteria.SportType,
                LanguageCode = CurrentSession.Language
            };

            switch ((SportType)criteria.SportType)
            {
                case SportType.VirtualHorseRacing:
                case SportType.VirtualGreyhounds:
                case SportType.VirtualSpeedway:
                case SportType.VirtualMotorRacing:
                case SportType.VirtualCycling:
                    var data = _reportService.GetMatchResultsVirtualHorseRacing(criteria.EventDate, criteria.SportType, leagueId, _includeTestData);
                    return data != null ? ResultEngine.HtmlParser(data, setting) : string.Empty;

                case SportType.LiveCasino:
                    return string.Empty;

                case SportType.NumberGame:
                    var numberGameData = _reportService.GetMatchResultsNumberGame(criteria.EventDate, leagueId, _includeTestData);
                    return numberGameData != null ? ResultEngine.HtmlParser(numberGameData, setting) : string.Empty;

                default:
                    var sportsBookData = _reportService.GetMatchResults(criteria.EventDate, criteria.SportType, leagueId, _includeTestData);
                    return sportsBookData != null ? ResultEngine.HtmlParser(sportsBookData, setting) : string.Empty;
            }
        }

        #endregion Utilities
    }
}