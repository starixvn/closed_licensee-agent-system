namespace LicenseeAgent.BetList.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Result.Core;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.BetList.ViewModels;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// BetListController class.
    /// </summary>
    public class BetListController : BaseController
    {
        #region Variables

        private readonly IBetListService _betlistService;
        private readonly ICoreService _coreService;
        private readonly IDownLinePermissionService _downLinePermissionService;

        #endregion Variables

        #region Constructors

        public BetListController()
            : this(null, null, null)
        {
        }

        public BetListController(
            IBetListService betlistService,
            ICoreService coreService,
            IDownLinePermissionService downLinePermissionService)
        {
            _betlistService = betlistService ?? new BetListService();
            _coreService = coreService ?? new CoreService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
        }

        #endregion Constructors

        #region Select By Member

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Member(int custId)
        {
            if (!_downLinePermissionService.IsDownLineMember(custId))
            {
                return RedirectToAccessDeniedPage();
            }

            var productList = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            ViewBag.PageContext = new PageContext(CurrentSession, custId);
            var model = new MemberViewModel
            {
                CustId = custId,
                ProductList = productList,
                IsAdmin = CurrentSession.IsAdmin
            };

            return View(model);
        }

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JSON object.
        /// </returns>
        [HttpPost]
        public JsonResult Member(MemberViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineMember(model.CustId))
            {
                return RedirectToAccessDeniedJson();
            }

            var balanceUp = _coreService.BalanceUpDay;
            var criteria = new SelectByMemberCriteria
            {
                CustId = model.CustId,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                StatusIds = model.Status,
                ProductIds = model.Product,
                AgentId = CurrentSession.AgentInfo.AgentId,
                MoveBetsDay = balanceUp.MoveBetsDay,
                BalanceUpDay = balanceUp.BalanceUpDay
            };
            var betListResult = _betlistService.GetByMember(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = betListResult,
                IsAdmin = CurrentSession.IsAdmin,
                IsViewPlayerCommission = CurrentSession.IsViewPlayerCommission()
            };

            var ticketBuilder = new BetListTicketBuilder(ticketBuilderParameters, criteria);
            return BetListJsonResult(ticketBuilder, betListResult);
        }

        #endregion Select By Member

        #region Total Bets

        /// <summary>
        /// Generate Bet List.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Return JSONRESULT.
        /// </returns>
        [HttpPost]
        public JsonResult TotalBets(TotalBetsViewModel model)
        {
            var criteria = new TotalBetsCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                BetTeam = model.BetTeam,
                BetType = model.BetType,
                MatchId = model.MatchId,
                ShowLiveTicket = model.ShowLiveTicket
            };
            var ticketResult = _betlistService.GetTotalBets(criteria);
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
                List = ticketTextList,
                TotalItem = criteria.TotalRows
            });
        }

        #endregion Total Bets

        #region Total Bets Parlay

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JSON object.
        /// </returns>
        [HttpPost]
        public JsonResult Parlay(TotalBetParlayViewModel model)
        {
            var criteria = new TotalBetsParlayCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                BetType = model.BetType
            };
            var ticketResult = _betlistService.GetParlay(criteria);
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
                List = ticketTextList,
                TotalItem = ticketTextList.Count
            });
        }

        #endregion Total Bets Parlay

        #region Win Loss

        /// <summary>
        /// Bet List for Win Loss.
        /// </summary>
        /// <param name="model">Bet List Win loss View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult WinLoss(WinLossViewModel model)
        {
            ViewBag.PageContext = new PageContext(CurrentSession, model.CustId);
            return View(model);
        }

        #endregion Win Loss

        #region Mix Parlay Details

        /// <summary>
        /// Mix Parlay Details.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Return a view.
        /// </returns>
        [HttpGet]
        public ActionResult MixParlayDetails(MixParlayDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelViewResult();
            }

            if (!_downLinePermissionService.IsDownLineTransaction(model.TransId, model.SportType, model.BetType))
            {
                return RedirectToAccessDeniedPage();
            }

            var criteria = new MixParlayDetailsCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                TransId = model.TransId
            };
            var ticketResult = _betlistService.GetMixParlayDetails(criteria);
            var builder = new BetListHTMLBuilder();
            var ticketHelper = new TicketHelper
            {
                IsShowOddsWithChoice = true
            };

            var stringBuilder = new StringBuilder();
            foreach (var ticket in ticketResult.MainTicketList)
            {
                var choice = builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false);
                stringBuilder.AppendFormat("<div class='ticketList'>{0}</div><div class='line'></div>", choice);
            }

            return Content(stringBuilder.ToString());
        }

        #endregion Mix Parlay Details

        #region System Parlay Details

        /// <summary>
        /// System Parlay Details.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// String content.
        /// </returns>
        [HttpGet]
        public ActionResult SystemParlayDetails(SystemParlayDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelViewResult();
            }

            if (!_downLinePermissionService.IsDownLineReferenceNumber(model.RefNo))
            {
                return RedirectToAccessDeniedPage();
            }

            var balanceUp = _coreService.BalanceUpDay;
            var criteria = new SystemParlayDetailsCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                RefNo = model.RefNo,
                BalanceUpDay = balanceUp.BalanceUpDay,
                MoveBetsDay = balanceUp.MoveBetsDay
            };

            var systemParlayData = _betlistService.GetSystemParlayResult(criteria);
            var builder = new BetListHTMLBuilder();
            var ticketHelper = new TicketHelper()
            {
                ShowSystemParlayPlayerComm = CurrentSession.IsViewPlayerCommission()
            };

            var systemParlayText = builder.BuildSystemParlayDetail(ticketHelper, systemParlayData);
            return Content(systemParlayText);
        }

        #endregion System Parlay Details

        #region Ticket Details

        /// <summary>
        /// Bet List Details.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Return a view.
        /// </returns>
        [HttpGet]
        public ActionResult Details(DetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelViewResult();
            }

            if (!_downLinePermissionService.IsDownLineTransaction(model.TransId, model.SportType, model.BetType))
            {
                return RedirectToAccessDeniedPage();
            }

            var criteria = new TicketDetailsCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                TransId = model.TransId,
                SportType = model.SportType,
                BetType = model.BetType
            };
            model.TicketDetails = _betlistService.GetDetails(criteria);
            model.IsDeposit = CurrentSession.AgentInfo.IsDeposit;
            return View(model);
        }

        #endregion Ticket Details

        #region Ticket Results

        /// <summary>
        /// Bet List Results.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// View result.
        /// </returns>
        public ActionResult Results(ResultViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelViewResult();
            }

            var setting = new ResultSetting
            {
                SportType = (SportType)model.SportType,
                IsOutRight = model.IsOutright,
                IsParlayPopUp = model.IsParlayPopup,
                IsPopUp = true,
                VisiblePopUpTitle = true,
                LanguageCode = CurrentSession.Language
            };
            var data = setting.IsParlayPopUp
                ? GetParlayResults(model.RefNoMixParlay.Value, setting)
                : GetNormalResults(model, setting);
            return View((object)data);
        }

        /// <summary>
        /// Gets the result normal data.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="setting">The setting.</param>
        /// <returns>Returns result string.</returns>
        [NonAction]
        public string GetNormalResults(ResultViewModel model, ResultSetting setting)
        {
            switch ((SportType)model.SportType)
            {
                case SportType.VirtualHorseRacing:
                case SportType.VirtualGreyhounds:
                case SportType.VirtualSpeedway:
                case SportType.VirtualMotorRacing:
                case SportType.VirtualCycling:
                    var racingData = _betlistService.GetVirtualRacingResults(model.MatchId.Value, model.RaceNo);
                    return racingData != null
                        ? ResultEngine.HtmlParser(racingData, setting)
                        : string.Empty;

                case SportType.LiveCasino:
                    return string.Empty;

                case SportType.NumberGame:
                    var numberGameData = _betlistService.GetNumberGameResults(model.MatchId.Value);
                    return numberGameData != null
                        ? ResultEngine.HtmlParser(numberGameData, setting)
                        : string.Empty;

                default:
                    var sportsBookData = _betlistService.GetSportsBookResults(model.MatchId.Value);
                    return sportsBookData != null
                        ? ResultEngine.HtmlParser(sportsBookData, setting)
                        : string.Empty;
            }
        }

        /// <summary>
        /// Get Parlay Results.
        /// </summary>
        /// <param name="refNo">Reference Number.</param>
        /// <param name="setting">Results Setting.</param>
        /// <returns>JSON object.</returns>
        [NonAction]
        public string GetParlayResults(long refNo, ResultSetting setting)
        {
            var criteria = new ResultsParlayCriteria
            {
                RefNo = refNo
            };
            var data = _betlistService.GetParlayResults(criteria);
            return data != null
                ? ResultEngine.HtmlParser(data, setting)
                : string.Empty;
        }

        #endregion Ticket Results

        #region Outstanding

        /// <summary>
        /// Outstanding - Bet List Report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Return a JSON object.
        /// </returns>
        /// Bet list result.
        [HttpPost]
        public JsonResult Outstanding(OutstandingViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineMember(model.CustId))
            {
                return RedirectToAccessDeniedJson();
            }

            var criteria = new MemberOutstandingCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                CustId = model.CustId
            };

            var betListResult = _betlistService.GetOutstanding(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = betListResult,
                IsAdmin = CurrentSession.IsAdmin,
                IsViewPlayerCommission = CurrentSession.IsViewPlayerCommission()
            };

            var ticketBuilder = new OutstandingBetListTicketBuilder(ticketBuilderParameters);
            return BetListJsonResult(ticketBuilder, betListResult);
        }

        #endregion Outstanding

        #region Outright

        /// <summary>
        /// Gets outright bet list result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns JSON object.</returns>
        [HttpPost]
        public JsonResult Outright(OutrightCriteria criteria)
        {
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var betListResult = _betlistService.GetOutright(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = betListResult,
                IsAdmin = CurrentSession.IsAdmin,
                IsViewPlayerCommission = CurrentSession.IsViewPlayerCommission()
            };

            var ticketBuilder = new TotalBetsTicketBuilder(ticketBuilderParameters);
            return BetListJsonResult(ticketBuilder, betListResult);
        }

        #endregion Outright

        #region Common

        /// <summary>
        /// Bets the list JSON result.
        /// </summary>
        /// <param name="ticketBuilder">The ticket builder.</param>
        /// <param name="betListResult">The bet list result.</param>
        /// <returns>JSON result.</returns>
        private JsonResult BetListJsonResult(ITicketBuilder ticketBuilder, BetListResult betListResult)
        {
            var ticketTextList = ticketBuilder.BuildTickets();
            var subTotalWinLost = betListResult.MainTicketList.Sum(x => x.Winlost);
            var subTotalComm = betListResult.MainTicketList.Sum(x => x.PlayerComm);
            return Json(new
            {
                List = ticketTextList,
                LegendType = CurrentSession.AgentInfo.LegendType,
                TotalItem = ticketTextList.Count(),
                SubTotalWinLost = subTotalWinLost,
                SubTotalComm = subTotalComm,
                TotalWinLost = subTotalWinLost + subTotalComm
            });
        }

        #endregion Common
    }
}