namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.TotalBets.Criteria;
    using LicenseeAgent.TotalBets.Entities;
    using LicenseeAgent.TotalBets.Services;
    using LicenseeAgent.TotalBets.ViewModels;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Outright controllers.
    /// </summary>
    public class OutrightController : CommonController
    {
        private readonly IOutrightService _outrightService;

        private readonly IBetListService _betlistService;

        public OutrightController()
            : this(null, null)
        {
        }

        public OutrightController(OutrightService outrightService, BetListService betlistService)
        {
            _outrightService = outrightService ?? new OutrightService();
            _betlistService = betlistService ?? new BetListService();
        }

        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.PageContext = new PageContext(CurrentSession, 0);
            var model = new OutrightViewModel { TimeInterval = 15 };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetOutrightData(OutrightCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var dataList = _outrightService.GetOutrightData(criteria);
            return Json(new
            {
                Result.SuccessValue,
                List = MapOutrightMainData(dataList)
            });
        }

        /// <summary>
        /// Get sport list.
        /// </summary>
        /// <param name="sport">Sport value.</param>
        /// <param name="checkedAll">Checked All.</param>
        /// <returns>Sport list.</returns>
        [HttpPost]
        public JsonResult GetSportList(string sport, bool checkedAll)
        {
            var criteria = new OutrightSportCriteria { AgentId = CurrentSession.AgentInfo.AgentId };
            var sports = _outrightService.GetOutrightSports(criteria).OrderBy(x => x.SportName);
            var sportIds = string.IsNullOrEmpty(sport) ? new List<string>() : sport.Split(',').ToList();
            var sportMultiItems = new List<dynamic>();
            foreach (var item in sports)
            {
                sportMultiItems.Add(new
                {
                    id = item.SportType,
                    value = item.SportType,
                    text = item.SportName,
                    @checked = checkedAll || sportIds.Contains(item.SportType.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = sportMultiItems,
            });
        }

        /// <summary>
        /// Get Leagues by sports.
        /// </summary>
        /// <param name="sport">Sport value.</param>
        /// <param name="league">League Type.</param>
        /// <param name="checkedAll">Checked All.</param>
        /// <returns>League list.</returns>
        [HttpPost]
        public JsonResult GetLeaguesBySports(string sport, string league, bool checkedAll)
        {
            var criteria = new OutrightLeagueCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportList = sport };
            var leagues = _outrightService.GetOutrightLeagues(criteria).OrderBy(x => x.LeagueName).ToList();
            return LeagueListToJson(league, checkedAll, leagues);
        }

        /// <summary>
        /// Get teams by leagues.
        /// </summary>
        /// <param name="sport">Sport value.</param>
        /// <param name="league">League value.</param>
        /// <param name="team">Team Type.</param>
        /// <param name="checkedAll">Checked All.</param>
        /// <returns>Team list.</returns>
        [HttpPost]
        public JsonResult GetTeamsByLeagues(string sport, string league, string team, bool checkedAll)
        {
            var criteria = new OutrightTeamCriteria { AgentId = CurrentSession.AgentInfo.AgentId, SportList = sport, LeagueList = league };
            var teams = _outrightService.GetOutrightTeam(criteria).OrderBy(x => x.TeamName).ToList();
            var teamIds = string.IsNullOrEmpty(team) ? new List<string>() : team.Split(',').ToList();
            var teamMultiItems = new List<dynamic>();
            foreach (var item in teams)
            {
                teamMultiItems.Add(new
                {
                    id = item.TeamId,
                    value = item.TeamId,
                    text = item.TeamName,
                    @checked = checkedAll || teamIds.Contains(item.TeamId.ToString()),
                    enable = true
                });
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = teamMultiItems,
            });
        }

        /// <summary>
        /// Gets the basketball bet list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns JSON object.</returns>
        [HttpPost]
        public JsonResult GetOutrightBetList(BetListOutrightCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var ticketResult = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = ticketResult,
                IsAdmin = CurrentSession.IsAdmin,
                IsViewPlayerCommission = CurrentSession.IsViewPlayerCommission()
            };

            var betListTicketBuilder = new TotalBetsTicketBuilder(ticketBuilderParameters);
            var ticketTextList = betListTicketBuilder.BuildTickets();
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = ticketTextList,
                TotalItem = criteria.TotalRows
            });
        }

        #region Utilities

        /// <summary>
        /// Map outright main data.
        /// </summary>
        /// <param name="dataList">Data list value.</param>
        /// <returns>Outright data result.</returns>
        private static IList<dynamic> MapOutrightMainData(IEnumerable<OutrightEntity> dataList)
        {
            IList<dynamic> sportItems = new List<dynamic>();
            var sports = dataList.Select(s => new { s.SportType, s.SportName }).Distinct().OrderBy(s => s.SportName).ToList();
            foreach (var sport in sports)
            {
                var allLeagueData = dataList.Where(l => l.SportType == sport.SportType).ToList();
                var leaguesInSport = allLeagueData.Select(l => new { l.LeagueId, l.LeagueName }).Distinct().OrderBy(l => l.LeagueName).ToList();
                var leagueItems = new List<dynamic>();
                foreach (var league in leaguesInSport)
                {
                    var teamsInLeague = allLeagueData.Where(t => t.LeagueId == league.LeagueId).ToList();
                    var leagueItem = new { LeagueId = league.LeagueId, LeagueName = league.LeagueName, TeamList = teamsInLeague };
                    leagueItems.Add(leagueItem);
                }

                var sportItem = new { SportType = sport.SportType, SportName = sport.SportName, LeagueList = leagueItems };
                sportItems.Add(sportItem);
            }

            return sportItems;
        }

        #endregion Utilities
    }
}