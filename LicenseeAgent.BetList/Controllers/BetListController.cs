namespace LicenseeAgent.BetList.Controllers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Result.Core;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.BetList.ViewModels;
    using LicenseeAgent.BetList.ViewResults;
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

        #region Index

        /// <summary>
        /// Bet List data.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns bet list data.</returns>
        [HttpPost]
        public JsonResult Index(BetListViewModel model)
        {
            var balanceUp = _coreService.BalanceUpDay;
            var criteria = new BetListSelByMemberCriteria
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
            var ticketResult = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = ticketResult,
                IsAdmin = CurrentSession.IsAdmin,
                IsViewPlayerCommission = CurrentSession.IsViewPlayerCommission()
            };

            var betListTicketBuilder = new BetListTicketBuilder(ticketBuilderParameters, criteria);
            var ticketTextList = betListTicketBuilder.BuildTickets();
            var subTotalWinLost = ticketResult.MainTicketList.Sum(x => x.Winlost);
            var subTotalComm = ticketResult.MainTicketList.Sum(x => x.PlayerComm);
            return Json(new
            {
                List = ticketTextList,
                LegendType = CurrentSession.AgentInfo.LegendType,
                IsLastLevel = CurrentSession.AgentInfo.IsLastLevel,
                TotalItem = criteria.TotalRows,
                SubTotalWinLost = subTotalWinLost,
                SubTotalComm = subTotalComm,
                TotalWinLost = subTotalWinLost + subTotalComm
            });
        }

        #endregion Index

        #region Total bets

        /// <summary>
        /// Generate Bet List.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Return JSONRESULT.
        /// </returns>
        [HttpPost]
        public JsonResult TotalBets(BetListTotalBetsViewModel model)
        {
            var criteria = new BetListTotalBetsCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                BetTeam = model.BetTeam,
                BetType = model.BetType,
                MatchId = model.MatchId,
                ShowLiveTicket = model.ShowLiveTicket
            };
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
                List = ticketTextList,
                TotalItem = criteria.TotalRows
            });
        }

        #endregion Total bets

        #region BetListSelByMember

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult BetListSelByMember(int custId)
        {
            if (!_downLinePermissionService.IsDownLineMember(custId))
            {
                return RedirectToAccessDeniedPage();
            }

            var productList = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            ViewBag.PageContext = new PageContext(CurrentSession, custId);
            var viewResult = new BetListSelectByMemberViewResult
            {
                CustId = custId,
                ProductList = productList,
                IsAdmin = CurrentSession.IsAdmin
            };

            return View(viewResult);
        }

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JSON object.
        /// </returns>
        [HttpPost]
        public JsonResult BetListSelByMember(BetListSelectByMemberViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineMember(model.CustId))
            {
                return RedirectToAccessDeniedJson();
            }

            var balanceUp = _coreService.BalanceUpDay;
            var criteria = new BetListSelByMemberCriteria
            {
                CustId = model.CustId,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                StatusIds = model.StatusIds,
                ProductIds = model.ProductIds,
                AgentId = CurrentSession.AgentInfo.AgentId,
                MoveBetsDay = balanceUp.MoveBetsDay,
                BalanceUpDay = balanceUp.BalanceUpDay
            };
            var ticketResult = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = ticketResult,
                IsAdmin = CurrentSession.IsAdmin,
                IsViewPlayerCommission = CurrentSession.IsViewPlayerCommission()
            };

            var betListTicketBuilder = new BetListTicketBuilder(ticketBuilderParameters, criteria);
            var ticketTextList = betListTicketBuilder.BuildTickets();
            var subTotalWinLost = ticketResult.MainTicketList.Sum(x => x.Winlost);
            var subTotalComm = ticketResult.MainTicketList.Sum(x => x.PlayerComm);
            return Json(new
            {
                List = ticketTextList,
                TotalItem = criteria.TotalRows,
                SubTotalWinLost = subTotalWinLost,
                SubTotalComm = subTotalComm,
                TotalWinLost = subTotalWinLost + subTotalComm
            });
        }

        #endregion BetListSelByMember

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="model">Bet List Win loss View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult BetListWinLossReport(BetListWinLossViewModel model)
        {
            ViewBag.PageContext = new PageContext(CurrentSession, model.CustId);
            return View(model);
        }

        /// <summary>
        /// Mix Parlay Details.
        /// </summary>
        /// <param name="transId">Transaction ID.</param>
        /// <param name="sportType">Sport Type.</param>
        /// <param name="betType">Bet Type of Transaction.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult MixParlayDetails(long transId, int sportType, int betType)
        {
            if (!_downLinePermissionService.IsDownLineTransaction(transId, sportType, betType))
            {
                return RedirectToAccessDeniedPage();
            }

            var agentId = CurrentSession.AgentInfo.AgentId;
            var criteria = new MixParlayDetailsCriteria { AgentId = agentId, TransId = transId };
            var ticketResult = _betlistService.GetMixParlayDetails(criteria);

            var builder = new BetListHTMLBuilder();
            var ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };

            var stringBuilder = new StringBuilder();
            foreach (var ticket in ticketResult.MainTicketList)
            {
                var choice = builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false);
                stringBuilder.AppendFormat("<div class='ticketList'>{0}</div><div class='line'></div>", choice);
            }

            return Content(stringBuilder.ToString());
        }

        /// <summary>
        /// System Parlay Result action.
        /// </summary>
        /// <param name="refNo">Reference number of transaction.</param>
        /// <returns>String content.</returns>
        [HttpGet]
        public ActionResult SystemParlayResult(long refNo)
        {
            if (!_downLinePermissionService.IsDownLineReferenceNumber(refNo))
            {
                return RedirectToAccessDeniedPage();
            }

            var agentId = CurrentSession.AgentInfo.AgentId;
            var balanceUp = _coreService.BalanceUpDay;
            var criteria = new SystemParlayResultCriteria
            {
                AgentId = agentId,
                RefNo = refNo,
                BalanceUpDay = balanceUp.BalanceUpDay,
                MoveBetsDay = balanceUp.MoveBetsDay
            };

            var systemParlayData = _betlistService.GetSystemParlayResult(criteria);
            var systemParlayText = BuildSystemParlayDetail(systemParlayData);

            return Content(systemParlayText);
        }

        /// <summary>
        /// Bet List Details.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Details(BetListDetailsCriteria criteria)
        {
            if (!_downLinePermissionService.IsDownLineTransaction(criteria.TransId, criteria.SportType, criteria.BetType))
            {
                return RedirectToAccessDeniedPage();
            }

            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var rawBetListDetails = _betlistService.GetBetListDetails(criteria);
            var viewModel = new BetListDetailsViewModel
            {
                BetListDetails = FormatRawBetListDetails(rawBetListDetails, criteria.Status),
                IsDeposit = CurrentSession.AgentInfo.IsDeposit
            };

            return View(viewModel);
        }

        /// <summary>
        /// Bet List Results.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JSON result.
        /// </returns>
        public ActionResult Results(BetListResultViewModel model)
        {
            var isParlayPopup =
                model.BetType == (int)LicenseeAgent.Core.Enumerations.BetType.BetType_9
                || model.BetType == (int)LicenseeAgent.Core.Enumerations.BetType.BetType_29;
            var setting = new ResultSetting
            {
                SportType = (SportType)model.SportType,
                IsOutRight = model.IsOutright,
                IsParlayPopUp = isParlayPopup,
                IsPopUp = true,
                IsShowPopUpTitle = true,
                LanguageCode = CurrentSession.Language
            };

            var data = string.Empty;
            if (setting.IsParlayPopUp)
            {
                data = GetParlayResults(model.RefNoMixParlay.Value, setting);
            }
            else
            {
                data = GetResultNormalData(model, setting);
            }

            return View((object)data);
        }

        /// <summary>
        /// Format raw bet list details.
        /// </summary>
        /// <param name="originalData">List of raw bet list details.</param>
        /// <param name="status">Ticket status ID.</param>
        /// <returns>List of formatted bet list details.</returns>
        public Collection<BetListDetails> FormatRawBetListDetails(IList<RawBetListDetails> originalData, string status)
        {
            var formattedData = new Collection<BetListDetails>();
            foreach (var item in originalData)
            {
                formattedData.Add(new BetListDetails
                {
                    Agent = item.AgentName,
                    PositionTaking = GenerateHtmlStringForAgentPT(item, status),
                    Commission = GenerateHtmlStringForAgentComm(item, status)
                });
            }

            return formattedData;
        }

        public string BuildSystemParlayDetail(ISystemParlayData systemParlayData)
        {
            var builder = new BetListHTMLBuilder();
            var ticketHelper = new TicketHelper()
            {
                ShowSystemParlayPlayerComm = CurrentSession.IsViewPlayerCommission()
            };

            var systemParlayText = builder.BuildSystemParlayDetail(ticketHelper, systemParlayData);
            return systemParlayText;
        }

        /// <summary>
        /// Gets the result normal data.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="setting">The setting.</param>
        /// <returns>Returns result string.</returns>
        private string GetResultNormalData(BetListResultViewModel model, ResultSetting setting)
        {
            switch ((SportType)model.SportType)
            {
                case SportType.VirtualHorseRacing:
                case SportType.VirtualGreyhounds:
                case SportType.VirtualSpeedway:
                case SportType.VirtualMotorRacing:
                case SportType.VirtualCycling:
                    var racingData = _betlistService.GetVirtualRacingResults(model.MatchId.Value, model.RaceNo);
                    return racingData != null ? ResultEngine.HtmlParser(racingData, setting) : string.Empty;

                case SportType.LiveCasino:
                    return string.Empty;

                case SportType.NumberGame:
                    var numberGameData = _betlistService.GetNumberGameResults(model.MatchId.Value);
                    return numberGameData != null ? ResultEngine.HtmlParser(numberGameData, setting) : string.Empty;

                default:
                    var sportsBookData = _betlistService.GetSportsBookResults(model.MatchId.Value);
                    return sportsBookData != null ? ResultEngine.HtmlParser(sportsBookData, setting) : string.Empty;
            }
        }

        /// <summary>
        /// Get Parlay Results.
        /// </summary>
        /// <param name="refNo">Reference Number.</param>
        /// <param name="setting">Results Setting.</param>
        /// <returns>JSON object.</returns>
        private string GetParlayResults(long refNo, ResultSetting setting)
        {
            var data = _betlistService.GetParlayResults(new BetListResultsParlayCriteria { RefNo = refNo });
            return data != null
                ? ResultEngine.HtmlParser(data, setting)
                : string.Empty;
        }

        private string GenerateHtmlStringForAgentPT(RawBetListDetails data, string status)
        {
            var colorValue = data.AgentPT >= 0 ? ">" : " class=\"red-text\">";
            var htmlString = "<div>" + string.Format("{0:#,##0.00}", data.AgentPTPercent * 100) + "%</div>";
            htmlString += (status == BetStatus.Running || status == BetStatus.Waiting)
                ? string.Empty
                : "<div" + colorValue + string.Format("{0:#,##0.00}", data.AgentPT) + "</div>";

            return htmlString;
        }

        private string GenerateHtmlStringForAgentComm(RawBetListDetails data, string status)
        {
            var htmlString = data.AgentLevel == 1
                ? string.Empty
                : "<div>" + string.Format("{0:#,##0.00}", data.AgentCommPercent * 100) + "%</div>";
            htmlString += (status == BetStatus.Running || status == BetStatus.Waiting)
                ? (data.AgentLevel == 1 ? "-" : string.Empty)
                : "<div>" + string.Format("{0:#,##0.00}", data.AgentComm) + "</div>";

            return htmlString;
        }
    }
}