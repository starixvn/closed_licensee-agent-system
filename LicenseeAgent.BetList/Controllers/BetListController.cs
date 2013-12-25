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
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Criterias;
    using Entities;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Caching;
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
        #endregion

        #region Constructor
        public BetListController(IBetListService betlistService)
        {
            _betlistService = betlistService;
            _cacheInstance = CacheService.CreateInstance(CacheType.MemoryCache);
        }
        #endregion

        #region Actions
        [HttpGet]
        public ActionResult Index(string someQueryHere)
        {
            return View();
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Returns Index page.</returns>
        [HttpPost]
        public JsonResult Index()
        {
            var criteria = new BetListCriteria(); // no SP's inputs temporarily
            var ticketResults = _betlistService.GetBetList(criteria);

            var model = new BetListViewModel()
            {
                TicketTextList = Ticket(ticketResults, criteria),
                TicketResult = ticketResults
            };

            var temp = Json(model.TicketTextList);
            return Json(model.TicketTextList);
        }

        /// <summary>
        /// Get bet list detail.
        /// </summary>
        /// <returns>Returns detail page.</returns>
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
            BetListCriteria criteria,
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

                ticketText.Status = ticketText.Status.Replace("onclick", "ng-click");

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }
        #endregion
    }
}
