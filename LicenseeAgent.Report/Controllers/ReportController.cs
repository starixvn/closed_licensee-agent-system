// ----------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/14">Created</change>
//   <change who="" date=""></change>
// </history>

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
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.BetList.ViewModels;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Report.Entities;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Services;
    using Services;
    using Utilities;
    using ViewModels;
    using Web.Controllers;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class ReportController : BaseController
    {
        #region Constant

        private bool _includeTestData = false;

        #endregion Constant

        #region Services

        private ICoreService _coreService;

        private IReportService _reportService;

        private IBetListService _betlistService;

        #endregion Services

        #region Constructor

        public ReportController(
            ICoreService coreService,
            IReportService reportService,
            IBetListService betlistService,
            IPermissionService permissionService)
            : base(permissionService)
        {
            _coreService = coreService;
            _reportService = reportService;
            _betlistService = betlistService;
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
            model.Sports = sports.Select(i => new SelectListItem { Text = i.SportName, Value = i.SportType }).OrderBy(j => j.Text).ToList();

            if (model.Sports.Count > 0)
            {
                model.Sports[0].Selected = true;
                var leagueCriteria = new LeagueCriteria()
                {
                    IsShowAll = criteria.IsShowAll,
                    EventDate = criteria.EventDate,
                    SportType = int.Parse(model.Sports[0].Value)
                };

                var leagues = _reportService.GetLeagues(leagueCriteria);
                model.Leagues = leagues.Select(i => new SelectListItem { Text = i.LeagueName, Value = i.LeagueID }).OrderBy(j => j.Text).ToList();

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
            var setting = new ResultSetting { IsOutRight = true, LanguageCode = "en-US" };

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
                    }).OrderBy(k => k.Text).ToList();
                var sportType = criteria.SportType == 0 ? int.Parse(sports[0].Value) : criteria.SportType;
                var leagues = outRightData.Where(i => i.SportType == sportType)
                    .Select(j => new SelectListItem { Text = DataRefWrapper.GetLeagueName(j.LeagueId), Value = j.LeagueId.ToString() })
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
            var leagueCriteria = new LeagueCriteria()
            {
                SportType = criteria.SportType,
                EventDate = criteria.EventDate
            };

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Results = GetMatchResultsData(leagueCriteria, criteria.LeagueId)
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
            var leagueItems = new List<SelectListItem>();
            var leagues = _reportService.GetLeagues(criteria);
            leagueItems = leagues.Select(i => new SelectListItem { Text = i.LeagueName, Value = i.LeagueID }).OrderBy(j => j.Text).ToList();

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
        public JsonResult GetBalanceUpMoveBet()
        {
            var balanceUp = _coreService.GetBalanceUpDay();
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
        /// <param name="agentId">Agent Id Info.</param>
        /// <param name="agentLevel">Agent Level.</param>
        /// <param name="breadCrumb">Bread Crumb.</param>
        /// <returns>Return a JSON object.</returns>
        public ActionResult Outstanding(int? agentId, int? agentLevel, string breadCrumb)
        {
            ViewBag.LoggedInAgentName = CurrentSession.AgentInfo.AgentName;
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
            criteria.AgentId = criteria.AgentId <= 0 ? CurrentSession.AgentInfo.AgentId : criteria.AgentId;
            var agentOutstandings = _reportService.GetOutstandings(criteria);

            decimal sumOutstanding = agentOutstandings.Sum(i => i.Outstanding);
            decimal sumPositiontaking = agentOutstandings.Sum(i => i.Positiontaking);

            return Json(new
            {
                List = agentOutstandings.ToList(),
                SumOutstanding = sumOutstanding,
                SumPositiontaking = sumPositiontaking,
                IsLastLevel = criteria.IsLastLevel
            });
        }

        /// <summary>
        /// Outstanding - Bet List Report.
        /// </summary>
        /// <param name="criteria">Outstanding Report Criteria.</param>
        /// <returns>Return a JSON object.</returns>
        [HttpPost]
        public JsonResult OutstandingBetList(OutstandingReportBetListCriteria criteria)
        {
            var betlistTickets = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters()
            {
                TicketResult = betlistTickets,
                IsAdmin = CurrentSession.IsAdmin
            };
            var betListTicketBuilder = new OutstandingBetListTicketBuilder(ticketBuilderParameters);

            var model = new BetListViewModel()
            {
                TicketTextList = betListTicketBuilder.BuildTickets(),
                TicketResult = betlistTickets
            };

            return Json(new
            {
                List = model.TicketTextList,
                IsLastLevel = CurrentSession.AgentInfo.IsLastLevel,
                TotalItem = model.TicketTextList.Count()
            });
        }

        #endregion

        #region Agent Win Loss Report

        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <param name="model">Agent Win Loss Report View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult AgentWinLoss(AgentWinLossReportViewModel model)
        {
            var isAdmin = CurrentSession.IsAdmin;
            model.AgentId = model.AgentId > 0 ? model.AgentId : CurrentSession.AgentInfo.AgentId;
            model.AgentLevel = model.AgentLevel > 0 ? model.AgentLevel : CurrentSession.AgentInfo.AgentLevel;            
            model.BreadCrumb = !string.IsNullOrEmpty(model.BreadCrumb)
                                ? model.BreadCrumb
                                : model.AgentId + "," + model.AgentLevel + "," + (isAdmin ? CurrentSession.AgentInfo.LicenseeName : CurrentSession.AgentInfo.UserName);
            model.Username = !string.IsNullOrEmpty(model.Username) ? model.Username : CurrentSession.AgentInfo.LicenseeName;

            ViewBag.PageContext = new PageContext(model.AgentId, model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel)
            {
                IsDirectDownline = CurrentSession.IsDirectDownline(model.AgentLevel),
                IsAdmin = CurrentSession.IsAdmin
            };
            return View(model);
        }

        /// <summary>
        /// Agent Win Loss.
        /// </summary>
        /// <param name="model">Agent Win Loss Report View Model.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetAgentWinLossData(AgentWinLossReportViewModel model)
        {
            var balanceUp = _coreService.GetBalanceUpDay();
            var result = new List<AgentWinLoss>();
            if (ModelHelper.IsLastAgent(model.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel))
            {
                var criteria = new WinLossMemberCriteria(model.AgentId, model.FromDate, model.ToDate, balanceUp.MoveBetsDay, balanceUp.BalanceUpDay, model.DrawFilter);
                result = _reportService.AgentWinLossReport(criteria).ToList();
            }
            else if (ModelHelper.IsAgentLevel0(model.AgentLevel))
            {
                var criteria = new WinLossSuperViewAgentCriteria(model.AgentId, model.FromDate, model.ToDate, balanceUp.MoveBetsDay, balanceUp.BalanceUpDay, model.DrawFilter);
                result = _reportService.AgentWinLossReport(criteria).ToList();
            }
            else
            {
                var criteria = new WinLossAgentViewAgentCriteria(model.AgentId, model.FromDate, model.ToDate, balanceUp.MoveBetsDay, balanceUp.BalanceUpDay, model.DrawFilter);
                result = _reportService.AgentWinLossReport(criteria).ToList();
            }

            var totalRow = from x in result
                           group x by 1
                           into g
                           select new
                           {
                               BetCount = g.Sum(x => x.BetCount),
                               Turnover = g.Sum(x => x.Turnover),
                               Winlost = g.Sum(x => x.Winlost),
                               PlayerComm = g.Sum(x => x.PlayerComm),
                               MemberTotal = g.Sum(x => x.MemberTotal),
                               DownlineStake = g.Sum(x => x.DownlineStake),
                               DownlineTotal = g.Sum(x => x.DownlineTotal),
                               AWinLost = g.Sum(x => x.AWinLost),
                               AComm = g.Sum(x => x.AComm),
                               ATotal = g.Sum(x => x.ATotal),
                               UplineStake = g.Sum(x => x.UplineStake),
                               UplineTotal = g.Sum(x => x.UplineTotal)
                           };
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Total = totalRow,
                List = result
            });
        }

        #endregion Agent Win Loss Report

        #region Utilities

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!this.CheckValidCurrency())
            {
                this.KickOut(filterContext);
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

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

        private static string ResultsMatchNoData()
        {
            var htmlNoDataStr = "<table class='fanex-result'><thead><tr class='row-header'><th class='column-kickofftime'>Kick Off. Time</th>";
            htmlNoDataStr += "<th class='column-match'>Match</th><th class='column-firsthaftscore'>First Half Score</th>";
            htmlNoDataStr += "<th class='column-finalscore'>Final Score</th><th class='column-status'>Status</th></tr></thead>";
            htmlNoDataStr += "<tbody><!--NoInfo--><tr class='row-noinfo'><td colspan='5'>There is no data</td></tr><!--/NoInfo--></tbody></table>";
            return htmlNoDataStr;
        }

        private static string ResultsOutrightNoData()
        {
            var htmlNoDataStr = "<table class='fanex-result'><thead><tr class='row-header'><th class='column-kickofftime'>Time</th>";
            htmlNoDataStr += "<th class='column-match'>Team</th><th class='column-status'>Resuls</th></tr></thead>";
            htmlNoDataStr += "<tbody><!--NoInfo--><tr class='row-noinfo'><td colspan='3'>There is no data</td></tr><!--/NoInfo--></tbody></table>";
            return htmlNoDataStr;
        }

        /// <summary>
        /// Get Match Results Data.
        /// </summary>
        /// <param name="criteria">Match Results Report Criteria.</param>
        /// <param name="leagueId">League ID input.</param>
        /// <returns>HTML string.</returns>
        private string GetMatchResultsData(LeagueCriteria criteria, int leagueId)
        {
            var matchResultsReportCriteria = new MatchResultsReportCriteria()
            {
                CustId = CurrentSession.OnUserId,
                SportType = criteria.SportType,
                LeagueId = leagueId,
                EventDate = criteria.EventDate
            };

            var setting = new ResultSetting
            {
                SportType = (SportType)criteria.SportType,
                LanguageCode = "en-US"
            };

            var data = _reportService.GetMatchResults(matchResultsReportCriteria);
            if (data != null)
            {
                return ResultEngine.HtmlParser(data, setting);
            }

            return string.Empty;
        }

        #endregion Utilities
    }
}