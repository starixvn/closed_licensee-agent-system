// ----------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Controllers
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using Criterias;
    using Entities;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Caching;
    using LicenseeAgent.Core.Services;
    using Services;
    using Utils;
    using ViewModels;
    using Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListController : BaseController
    {
        #region Constant

        private const string BetListCustomerInfo = "BetListCustomerInfo";

        private const int CacheDuration = 43200;

        private ICache _cacheInstance;

        #endregion

        #region Services

        private IBetListService _betlistService;

        private ICoreService _coreService;

        #endregion

        #region Constructor

        public BetListController(IBetListService betlistService, ICoreService coreService)
        {
            _betlistService = betlistService;
            _coreService = coreService;
            _cacheInstance = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        #endregion

        #region Actions

        /// <summary>
        /// Index page of Bet List.
        /// </summary>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult BetListSelByMember()
        {
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
            criteria.AgentId = 1000000519;
            criteria.CustId = 13467694;
            criteria.MoveBetsDay = balanceUp.MoveBetDay;
            criteria.BalanceUpDay = balanceUp.BalanceUpDay;
            criteria.StatusId = criteria.StatusId == -1 ? null : criteria.StatusId;

            var ticketResults = _betlistService.GetBetList(criteria);

            var model = new BetListViewModel()
            {
                TicketTextList = Ticket(ticketResults, criteria),
                TicketResult = ticketResults
            };

            return Json(new
            {
                List = model.TicketTextList,
                TotalItem = criteria.TotalRows
            });
        }

        /// <summary>
        /// Mix Parlay Details.
        /// </summary>
        /// <param name="transId">Transaction ID.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult MixParlayDetails(long transId)
        {
            var agentId = CurrentSession.AgentInfo.AgentId;
            var criteria = new MixParlayDetailsCriteria { AgentId = agentId, TransId = transId };
            var ticketResult = _betlistService.GetMixParlayDetails(criteria);
            var builder = new BetListHTMLBuilder();
            var ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };

            var stringBuilder = new StringBuilder();
            foreach (var ticket in ticketResult.MainTicketList)
            {
                var choice = builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false);
                stringBuilder.AppendFormat("<div>{0}</div><div class='line'></div>", choice);
            }

            return Content(stringBuilder.ToString());
        }

        /// <summary>
        /// Bet List Details.
        /// </summary>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Details()
        {
            var fakeData = new BetListDetailsViewModel();
            fakeData.BetListDetails.Add(new BetListDetails
                {
                    Agent = "MNS",
                    PositionTaking = "<div>20%</div><div>100.00</div>",
                    Commission = "<div>5%</div><div>7.00</div>"
                });

            return View(fakeData);
        }

        #endregion

        #region Utilities
        /// <summary>
        /// Generate ticket text.
        /// </summary>
        /// <param name="ticketResult">Ticket result Parameter value.</param>
        /// <param name="criteria"> Parameter value.</param>
        /// <param name="exchangeRate">Exchange rate value.</param>
        /// <param name="startIndex">Start index Parameter value.</param>
        /// <param name="showOdds">Show Odds or not.</param>
        /// <returns>Result value.</returns>
        [NonAction]
        public List<TicketText> Ticket(
            TicketResult ticketResult,
            BetListCriteriaBase criteria,
            decimal exchangeRate = 1,
            int startIndex = 1,
            bool showOdds = false)
        {
            var builder = new BetListHTMLBuilder();
            var ticketTextList = new List<TicketText>();
            var ticketHelper = new TicketHelper();
            var baseCurrency = criteria == null ? string.Empty : criteria.BaseCurrency;

            foreach (var ticket in ticketResult.MainTicketList)
            {
                if (ticket.BetTypeId == (int)BetType.CombinationMixParlay)
                {
                    var custid = ticket.CustId != 0 ? ticket.CustId : criteria.CustId;
                    var formatDetail = @"showCombMPDetail({0},{1},{2},{3},&quot;{4}&quot;,&quot;{5}&quot;)";
                    var betTeam = ticketHelper.GetBetTypeNameById(ticket.BetTypeId) + " - " + ticketHelper.GetCombinationMPOfferName(ticket.BetTeam);
                    ticketHelper.ClientDetailSystemParlayFunction = string.Format(
                        formatDetail,
                        ticket.TransId,
                        ticket.RefNo,
                        ticket.BetId,
                        custid,
                        betTeam,
                        baseCurrency);
                }

                ticketHelper.IsShowOddsWithChoice = showOdds;
                ticket.Stake *= exchangeRate;
                ticket.ActualStake *= exchangeRate;
                var ticketText = new TicketText
                {
                    Index = startIndex++,
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = builder.BuildTrans(ticket),
                    Status = builder.BuildLasBetListResultStatus(
                        ticket, ticketHelper, ticketResult.TicketDataList, criteria, 20, "UUS"),
                    Choice = builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false),
                    Odds = builder.BuildOdds(ticket, ticketResult.TicketDataList, CacheRef.GetOddsTypeName),
                    Stake = builder.BuildStake(ticket),
                    AgentPT = builder.BuildAgentPT(ticket),
                    AgentComm = builder.BuildAgentComm(ticket),
                    WinLoss = string.Format("{0:#,##0.00}", ticket.Winlost * exchangeRate)
                };

                ticketText.Feature = "<a class=\"iplink\" href=\"javascript:void(0);\" onclick=\"javascript: POPUP.openDialogFrame('BetList/Details?";
                ticketText.Feature += "SomeParamShouldBeHere=1', { width: 360, height: 280 }); \">Feature</a>";

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }
        #endregion
    }
}
