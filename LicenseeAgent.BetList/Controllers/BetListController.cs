// ----------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Controllers
{
    using System.Web.Mvc;
    using Entities;
    using Fanex.Result.Core;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Services;
    using Services;
    using ViewModels;

    /// <summary>
    /// BetListController class.
    /// </summary>
    public class BetListController : BetListBase
    {
        private readonly IBetListService _betlistService;
        private readonly ICoreService _coreService;
        private readonly IDownlLinePermissionService _permissionService;

        public BetListController(
            IBetListService betlistService,
            ICoreService coreService,
            IDownlLinePermissionService permissionService)
            : base(permissionService)
        {
            _betlistService = betlistService;
            _coreService = coreService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult BetListSelByMember(int custId)
        {
            ViewBag.CustId = custId;
            ViewBag.ProductList = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            return View();
        }

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="criteria">Bet List By Member Criteria.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult BetListSelByMember(BetListSelByMemberCriteria criteria)
        {
            var balanceUp = _coreService.GetBalanceUpDay();
            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            criteria.MoveBetsDay = balanceUp.MoveBetsDay;
            criteria.BalanceUpDay = balanceUp.BalanceUpDay;
            criteria.StatusIDs = criteria.StatusIDs == "255" ? null : criteria.StatusIDs; // All

            var ticketResults = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = ticketResults,
                IsAdmin = CurrentSession.IsAdmin
            };
            var betListTicketBuilder = new BetListTicketBuilder(ticketBuilderParameters, criteria);

            var model = new BetListViewModel
            {
                TicketTextList = betListTicketBuilder.BuildTickets(),
                TicketResult = ticketResults
            };

            return Json(new
            {
                List = model.TicketTextList,
                IsLastLevel = CurrentSession.AgentInfo.IsLastLevel,
                TotalItem = criteria.TotalRows
            });
        }

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <param name="model">Bet List Win loss View Model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult BetListWinLossReport(BetListWinLossViewModel model)
        {
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
            if (!_permissionService.IsDownLineTransaction(transId, sportType, betType))
            {
                return RedirectToAccessDeniedPage();
            }

            var agentId = CurrentSession.AgentInfo.AgentId;
            var criteria = new MixParlayDetailsCriteria { AgentId = agentId, TransId = transId };
            var ticketResult = _betlistService.GetMixParlayDetails(criteria);

            return BuildMixParlayDetails(ticketResult);
        }

        /// <summary>
        /// System Parlay Result action.
        /// </summary>
        /// <param name="refNo">Reference number of transaction.</param>
        /// <returns>String content.</returns>
        [HttpGet]
        public ActionResult SystemParlayResult(long refNo)
        {
            if (!_permissionService.IsDownLineReferenceNumber(refNo))
            {
                return RedirectToAccessDeniedPage();
            }

            var agentId = CurrentSession.AgentInfo.AgentId;
            var balanceUp = _coreService.GetBalanceUpDay();

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
            if (!_permissionService.IsDownLineTransaction(criteria.TransId, criteria.SportType, criteria.BetType))
            {
                return RedirectToAccessDeniedPage();
            }

            criteria.AgentId = CurrentSession.AgentInfo.AgentId;
            var rawBetListDetails = _betlistService.GetBetListDetails(criteria);
            var viewModel = new BetListDetailsViewModel
            {
                BetListDetails = FormatRawBetListDetails(rawBetListDetails, criteria.Status)
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
        /// Gets the result normal data.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="setting">The setting.</param>
        /// <returns>Returns result string.</returns>
        private string GetResultNormalData(BetListResultViewModel model, ResultSetting setting)
        {
            switch ((SportType)model.SportType)
            {
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
    }
}