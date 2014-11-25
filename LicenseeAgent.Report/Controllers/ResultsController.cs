namespace LicenseeAgent.Report.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Result.Core;
    using Fanex.Result.Core.DTO;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Report.Criteria.Results;
    using LicenseeAgent.Report.Entities.Results;
    using LicenseeAgent.Report.Services;
    using LicenseeAgent.Report.ViewModels.Results;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class ResultsController : BaseController
    {
        #region Services

        private readonly ICoreService _coreService;
        private readonly IResultsService _resultsService;

        #endregion Services

        #region Constructor

        public ResultsController()
            : this(null, null)
        {
        }

        public ResultsController(
            ICoreService coreService,
            IResultsService reportService)
        {
            _coreService = coreService ?? new CoreService();
            _resultsService = reportService ?? new ResultsService();
        }

        #endregion Constructor

        #region Index

        /// <summary>
        /// Index page of Results report.
        /// </summary>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var balanceUp = _coreService.BalanceUpDay;
            var matchResultsViewModel = new MatchResultsViewModel
            {
                BalanceUpDay = balanceUp.BalanceUpDay,
                Date = DateTime.Today
            };
            var outrightResultsViewModel = new OutrightResultsViewModel
            {
                BalanceUpDay = balanceUp.BalanceUpDay,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today
            };

            var model = new ResultsViewModel
            {
                MatchResults = matchResultsViewModel,
                OutrightResults = outrightResultsViewModel
            };

            return View(model);
        }

        #endregion Index

        #region Match results

        /// <summary>
        /// Index page of Results report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Return a view.
        /// </returns>
        [HttpPost]
        public JsonResult Match(MatchResultsViewModel model)
        {
            IList<SportEntity> sports = null;
            if (model.Sport == 0)
            {
                sports = GetMatchSports(model);
            }

            IList<LeagueEntity> leagues = null;
            if (model.League == 0 && model.Sport != 0)
            {
                leagues = GetMatchLeagues(model);
            }

            var results = model.Sport != 0
                ? GetMatchResults(model)
                : ResultsNoData();
            return Json(new
            {
                Sports = sports,
                Leagues = leagues,
                Results = results
            });
        }

        /// <summary>
        /// Gets the match sports.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns list of sports.</returns>
        [NonAction]
        public IList<SportEntity> GetMatchSports(MatchResultsViewModel model)
        {
            var criteria = new GetSportCriteria
            {
                EventDate = model.Date,
                IsShowAll = model.IsShowAll
            };

            var sports = _resultsService.GetSports(criteria);
            sports.ToList().ForEach(x =>
            {
                x.SportName = DataRefWrapper.GetSportName(x.SportType);
            });

            if (sports.Count > 0)
            {
                model.Sport = sports[0].SportType;
            }

            return sports;
        }

        /// <summary>
        /// Gets the match leagues.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns list of leagues.</returns>
        [NonAction]
        public IList<LeagueEntity> GetMatchLeagues(MatchResultsViewModel model)
        {
            var criteria = new GetLeagueCriteria
            {
                IsShowAll = model.IsShowAll,
                EventDate = model.Date,
                SportType = model.Sport
            };

            var leagues = _resultsService.GetLeagues(criteria);
            leagues.ToList().ForEach(x =>
            {
                x.LeagueName = DataRefWrapper.GetLeagueName(x.LeagueId);
            });

            if (leagues.Count > 0)
            {
                leagues.Insert(
                    0,
                    new LeagueEntity
                    {
                        LeagueId = 0,
                        LeagueName = Las.AllLeagues
                    });
                model.League = leagues[0].LeagueId;
            }

            return leagues;
        }

        /// <summary>
        /// Get Match Results Data.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// HTML string.
        /// </returns>
        [NonAction]
        public string GetMatchResults(MatchResultsViewModel model)
        {
            var setting = new ResultSetting
            {
                SportType = (SportType)model.Sport,
                LanguageCode = CurrentSession.Language
            };
            switch ((SportType)model.Sport)
            {
                case SportType.VirtualHorseRacing:
                case SportType.VirtualGreyhounds:
                case SportType.VirtualSpeedway:
                case SportType.VirtualMotorRacing:
                case SportType.VirtualCycling:
                    var virtualHorseRacingCriteria = new MatchResultsVirtualHorseRacingCriteria
                    {
                        IsShowAll = model.IsShowAll,
                        EventDate = model.Date,
                        LeagueId = model.League,
                        SportType = model.Sport
                    };
                    var data = _resultsService.GetMatchResultsVirtualHorseRacing(virtualHorseRacingCriteria);
                    return data != null ? ResultEngine.HtmlParser(data, setting) : string.Empty;

                case SportType.LiveCasino:
                    return string.Empty;

                case SportType.NumberGame:
                    var numberGameCriteria = new MatchResultsNumberGameCriteria
                    {
                        IsShowAll = model.IsShowAll,
                        EventDate = model.Date,
                        LeagueId = model.League
                    };
                    var numberGameData = _resultsService.GetMatchResultsNumberGame(numberGameCriteria);
                    return numberGameData != null ? ResultEngine.HtmlParser(numberGameData, setting) : string.Empty;

                default:
                    var sportsBookCriteria = new MatchResultsSportsBookCriteria
                    {
                        IsShowAll = model.IsShowAll,
                        EventDate = model.Date,
                        SportType = model.Sport,
                        LeagueId = model.League
                    };
                    var sportsBookData = _resultsService.GetMatchResultsSportsBook(sportsBookCriteria);
                    return sportsBookData != null ? ResultEngine.HtmlParser(sportsBookData, setting) : string.Empty;
            }
        }

        #endregion Match results

        #region Outright results

        /// <summary>
        /// Index page of Results report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Return a view.
        /// </returns>
        [HttpPost]
        public JsonResult Outright(OutrightResultsViewModel model)
        {
            var criteria = new OutrightResultsReportCriteria
            {
                IsShowAll = model.IsShowAll,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                LeagueId = model.League,
                SportType = model.Sport
            };
            var outRightData = _resultsService.GetOutrightResults(criteria);

            IList<SportEntity> sports = null;
            IList<LeagueEntity> leagues = null;
            var results = string.Empty;
            if (outRightData.Count > 0)
            {
                if (model.Sport == 0)
                {
                    sports = GetOutrightSports(outRightData);
                }

                var sportType = model.Sport == 0 ? sports[0].SportType : model.Sport;
                if (model.League == 0 && sportType != 0)
                {
                    leagues = GetOutrightLeagues(outRightData, sportType);
                }

                if (sportType != 0)
                {
                    results = GetOutrightResults(outRightData, sportType, model.League);
                }
            }

            return Json(new
            {
                Sports = sports,
                Leagues = leagues,
                Results = string.IsNullOrEmpty(results) ? ResultsNoData() : results
            });
        }

        [NonAction]
        public IList<SportEntity> GetOutrightSports(IList<OutRight> data)
        {
            return data.GroupBy(i => i.SportType).Select(x => new SportEntity
            {
                SportType = x.First().SportType,
                SportName = DataRefWrapper.GetSportName(x.First().SportType)
            }).OrderBy(x => x.SportType).ToList();
        }

        /// <summary>
        /// Gets the outright leagues.
        /// </summary>
        /// <param name="data">The outright data.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <returns>Returns list of leagues.</returns>
        [NonAction]
        public IList<LeagueEntity> GetOutrightLeagues(IList<OutRight> data, int sportType)
        {
            var leagues = data.Where(x => x.SportType == sportType).GroupBy(x => x.LeagueId).Select(x => new LeagueEntity
            {
                LeagueId = x.First().LeagueId,
                LeagueName = DataRefWrapper.GetLeagueName(x.First().LeagueId)
            }).OrderBy(x => x.LeagueName).ToList();
            if (leagues.Count > 0)
            {
                leagues.Insert(
                    0,
                    new LeagueEntity
                    {
                        LeagueId = 0,
                        LeagueName = Las.AllLeagues
                    });
            }

            return leagues;
        }

        /// <summary>
        /// Gets the outright result.
        /// </summary>
        /// <param name="data">The outright data.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="leagueId">The league identifier.</param>
        /// <returns>Returns outright result.</returns>
        [NonAction]
        public string GetOutrightResults(IList<OutRight> data, int sportType, int leagueId)
        {
            var filteredData = data.Where(i => i.SportType == sportType && (leagueId == 0 || leagueId == i.LeagueId)).ToList();
            if (data.Count > 0)
            {
                var setting = new ResultSetting
                {
                    IsOutRight = true,
                    LanguageCode = LanguageManager.Language
                };
                return ResultEngine.HtmlParser(filteredData, setting);
            }

            return string.Empty;
        }

        #endregion Outright results

        #region Utilities

        /// <summary>
        /// Return match no data.
        /// </summary>
        /// <returns>HTML string.</returns>
        private string ResultsNoData()
        {
            return RenderRazorViewToString("~/Areas/Report/Views/Results/_NoData.cshtml");
        }

        #endregion Utilities
    }
}