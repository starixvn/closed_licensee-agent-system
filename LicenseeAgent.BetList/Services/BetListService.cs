// ----------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Core.Services;
    using DataAccess;
    using Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.NAP.Framework.Data;
    using Fanex.Result.Core.DTO;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.Core.Exceptions;

    /// <summary>
    /// BetListService class.
    /// </summary>
    public class BetListService : QueryService, IBetListService
    {
        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly IBetListDataAccess _betlistDataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        public BetListService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        /// <param name="betlistDataAccess">Account data access.</param>
        private BetListService(IBetListDataAccess betlistDataAccess)
        {
            _betlistDataAccess = betlistDataAccess ?? new BetListDataAccess();
        }

        /// <summary>
        /// Get Bet List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        public TicketResult GetBetList(ICriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                return _betlistDataAccess.GetBetList(criteria);
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message);
            }
        }

        /// <summary>
        /// Get Bet List Parlay.
        /// </summary>
        /// <param name="criteria">Mix Parlay Details Criteria.</param>
        /// <returns>Ticket results.</returns>
        public TicketResult GetMixParlayDetails(MixParlayDetailsCriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                return _betlistDataAccess.GetMixParlayDetails(criteria);
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message);
            }
        }

        /// <summary>
        /// Get system parlay result data.
        /// </summary>
        /// <param name="criteria">System parlay result criteria.</param>
        /// <returns>System Parlay Data.</returns>
        public SystemParlayData GetSystemParlayResult(SystemParlayResultCriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                return _betlistDataAccess.GetSystemParlayResult(criteria);
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message);
            }
        }

        /// <summary>
        /// Get Bet List Details.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>A list of bet list details.</returns>
        public IList<RawBetListDetails> GetBetListDetails(BetListDetailsCriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                return GetEntites<RawBetListDetails>(criteria);
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message);
            }
        }

        /// <summary>
        /// Get Parlay List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        public TicketResult GetParlayList(ICriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                return _betlistDataAccess.GetParlayList(criteria);
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message);
            }
        }

        /// <summary>
        /// Get Sports book Results.
        /// </summary>
        /// <param name="matchId">Result Match ID.</param>
        /// <returns>
        /// The list of matches.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Invalid criteria.</exception>
        /// <exception cref="CriteriaException"></exception>
        public IList<NumberGame> GetNumberGameResults(int matchId)
        {
            var criteria = new BetListResultsNumberGameCriteria
            {
                MatchId = matchId
            };

            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                var dataTable = _betlistDataAccess.GetBetListResults(criteria);
                return BindingNumberGame(dataTable);
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message);
            }
        }

        /// <summary>
        /// Gets the sports book results.
        /// </summary>
        /// <param name="matchId">The match identifier.</param>
        /// <returns>Return number game result.</returns>
        /// <exception cref="System.InvalidOperationException">Invalid criteria.</exception>
        /// <exception cref="CriteriaException"></exception>
        public IList<Sportbook> GetSportsBookResults(int matchId)
        {
            var criteria = new BetListResultsSportsBookCriteria
            {
                MatchId = matchId
            };

            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                var dataTable = _betlistDataAccess.GetBetListResults(criteria);
                return BindSportsBook(dataTable);
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message);
            }
        }

        /// <summary>
        /// Get Parlay Results.
        /// </summary>
        /// <param name="criteria">Bet List Results Parlay Criteria.</param>
        /// <returns>The parlay list.</returns>
        public IList<Parlay> GetParlayResults(BetListResultsParlayCriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                var dataTable = _betlistDataAccess.GetBetListResults(criteria);
                return BindResults(dataTable, criteria.RefNo);
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message);
            }
        }

        #region Biding

        /// <summary>
        /// Mapping data to Sport-book DTO.
        /// </summary>
        /// <param name="data">The data table.</param>
        /// <returns>The sport-book data list.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1125:UseShorthandForNullableTypes", Justification = "Reviewed.")]
        private static IList<Sportbook> BindSportsBook(DataTable data)
        {
            if (data != null && data.Rows.Count > 0)
            {
                return
                    data.AsEnumerable()
                        .Select(
                            i =>
                                new Sportbook
                                {
                                    AwayId = int.Parse(i["awayID"].ToString()),
                                    HomeId = int.Parse(i["homeID"].ToString()),
                                    BetStatus =
                                        i["betstatus"] != DBNull.Value ? i["betstatus"].ToString() : string.Empty,
                                    LeagueId = int.Parse(i["leagueid"].ToString()),
                                    MatchId = int.Parse(i["matchid"].ToString()),
                                    EventStatus = i["eventstatus"].ToString(),
                                    FinalAwayScore =
                                        i["finalawayscore"] != DBNull.Value
                                            ? int.Parse(i["finalawayscore"].ToString())
                                            : new Nullable<int>(),
                                    HtAwayScore =
                                        i["htawayscore"] != DBNull.Value ? int.Parse(i["htawayscore"].ToString()) : new Nullable<int>(),
                                    Color = i.Table.Columns.Contains("lcolor") && i["lcolor"] != DBNull.Value ? i["lcolor"].ToString() : string.Empty,
                                    FinalHomeScore =
                                        i["finalhomescore"] != DBNull.Value
                                            ? int.Parse(i["finalhomescore"].ToString())
                                            : new Nullable<int>(),
                                    HtHomeScore =
                                        i["hthomescore"] != DBNull.Value
                                            ? int.Parse(i["hthomescore"].ToString())
                                            : new Nullable<int>(),
                                    EventDate = DateTime.Parse(i["eventdate"].ToString()),
                                    IsNeutral = bool.Parse(i["IsNeutral"].ToString()),
                                    IsPending = bool.Parse(i["IsPending"].ToString()),
                                    KickOffTime = DateTime.Parse(i["kickofftime"].ToString()),
                                    MatchCode = i["matchcode"].ToString(),
                                    GoalRecord = i["goalrecord"].ToString(),
                                    ResultValue = string.Empty,
                                    IsHTCal = Convert.ToBoolean(i["IsHTCal"]),
                                    Calculate = Convert.ToBoolean(i["Calculate"]),
                                    RefNo = string.Empty
                                })
                        .ToList();
            }

            return new List<Sportbook>();
        }

        /// <summary>
        /// Mapping data to NG DTO.
        /// </summary>
        /// <param name="dataNg">The data table.</param>
        /// <returns>The NG data list.</returns>
        private static IList<NumberGame> BindingNumberGame(DataTable dataNg)
        {
            if (dataNg != null && dataNg.Rows.Count > 0)
            {
                dataNg.AsEnumerable().OrderBy(i => i["leagueId"]).OrderBy(i => i["matchid"]).OrderBy(i => i["seq"]);
                var dataListNg = new List<NumberGame>();
                foreach (var mtc in dataNg.AsEnumerable())
                {
                    var matchId = int.Parse(mtc["matchid"].ToString());
                    var existMatch = dataListNg.SingleOrDefault(i => i.MatchId == matchId);
                    if (existMatch != null)
                    {
                        existMatch.Seg.Add(int.Parse(mtc["seq"].ToString()));
                        existMatch.Ball.Add(int.Parse(mtc["number"].ToString()));
                    }
                    else
                    {
                        dataListNg.Add(
                            new NumberGame
                            {
                                LeagueId = int.Parse(mtc["leagueid"].ToString()),
                                MatchId = int.Parse(mtc["matchid"].ToString()),
                                EventStatus = mtc["eventstatus"].ToString(),
                                EventDate = DateTime.Parse(mtc["eventdate"].ToString()),
                                IsPending = mtc.Table.Columns.Contains("isrefund") && mtc["isrefund"].ToString() != "0",
                                KickOffTime = DateTime.Parse(mtc["kickofftime"].ToString()),
                                MatchCode = mtc["matchcode"].ToString(),
                                Seg = new List<int> { int.Parse(mtc["seq"].ToString()) },
                                Ball = new List<int> { int.Parse(mtc["number"].ToString()) }
                            });
                    }
                }

                return dataListNg;
            }

            return new List<NumberGame>();
        }

        /// <summary>
        /// Mapping data to Live casino DTO.
        /// </summary>
        /// <param name="dataLc">The data table.</param>
        /// <returns>The live casino data list.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1125:UseShorthandForNullableTypes", Justification = "Reviewed.")]
        private static IList<LiveCasino> BidingLiveCasinos(DataTable dataLc)
        {
            if (dataLc != null && dataLc.Rows.Count > 0)
            {
                return
                    dataLc.AsEnumerable()
                        .Select(
                            i =>
                                new LiveCasino
                                {
                                    ShowTime = i["showtime"].ToString(),
                                    LeagueId = int.Parse(i["leagueid"].ToString()),
                                    EventStatus = i["eventstatus"].ToString(),
                                    FinalAwayScore =
                                        i["finalawayscore"] != DBNull.Value ? int.Parse(i["finalawayscore"].ToString()) : new Nullable<int>(),
                                    FinalHomeScore =
                                        i["finalhomescore"] != DBNull.Value ? int.Parse(i["finalhomescore"].ToString()) : new Nullable<int>(),
                                    EventDate = DateTime.Parse(i["eventdate"].ToString()),
                                    IsPending = bool.Parse(i["IsPending"].ToString()),
                                    KickOffTime = DateTime.Parse(i["kickofftime"].ToString()),
                                    IsHTCal = Convert.ToBoolean(i["IsHTCal"]),
                                    Calculate = Convert.ToBoolean(i["Calculate"]),
                                    BetStatus = i["betstatus"] != DBNull.Value ? i["betstatus"].ToString() : string.Empty,
                                    Bettype = int.Parse(i["homeid2"].ToString())
                                })
                        .ToList();
            }

            return new List<LiveCasino>();
        }

        /// <summary>
        /// Mapping racing data from data table.
        /// </summary>
        /// <param name="dataRc">The data table.</param>
        /// <returns>The racing data list.</returns>
        private static IList<Racing> BidingRacing(DataTable dataRc)
        {
            if (dataRc != null && dataRc.Rows.Count > 0)
            {
                return
                    dataRc.AsEnumerable()
                        .Select(
                            i =>
                                new Racing
                                {
                                    LeagueId = int.Parse(i["meetingid"].ToString()),
                                    EngineId = i["HorseId"].ToString(),
                                    EngineNo = i["HorseNo"].ToString(),
                                    IsFavor = i["IsFavor"] != DBNull.Value && i["IsFavor"].ToString().Equals("1") ? true : false,
                                    IsSCR = i["IsSCR"] != DBNull.Value && i["IsSCR"].ToString().Equals("1") ? true : false,
                                    Place = int.Parse(i["place"].ToString()),
                                    PlaceOdds = decimal.Parse(i["placeodds"].ToString()),
                                    WinOdds = decimal.Parse(i["odds"].ToString()),
                                    RaceName = i["racename"] != DBNull.Value ? int.Parse(i["racename"].ToString()) : 0,
                                    KickOffTime = i["kickofftime"] != DBNull.Value ? DateTime.Parse(i["kickofftime"].ToString()) : DateTime.MinValue,
                                    Favourite = i["IsFavor"] != DBNull.Value ? i["IsFavor"].ToString() : string.Empty,
                                    RowCount = int.Parse(i["rowcount"].ToString()),
                                    SCROfMatch = i["SCR"].ToString(),
                                    TFOfMatch = i["TF"].ToString()
                                })
                        .ToList();
            }

            return new List<Racing>();
        }

        /// <summary>
        /// Get parlay match for pop up page.
        /// </summary>
        /// <param name="data">Data Table.</param>
        /// <param name="refNo">The reference no.</param>
        /// <returns>
        /// The parlay data list.
        /// </returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private static IList<Parlay> BindResults(DataTable data, long refNo)
        {
            if (data != null && data.Rows.Count > 0)
            {
                var dataList = data.AsEnumerable();
                var dataParlay = new List<Parlay>();

                foreach (var row in dataList)
                {
                    var leagueId = int.Parse(row["LeagueID"].ToString());
                    if (dataParlay.All(i => i.LeagueId != leagueId))
                    {
                        dataParlay.Add(
                            new Parlay
                            {
                                LeagueId = leagueId,
                                SportId = int.Parse(row["sporttype"].ToString()),
                                Sportbook =
                                    dataList.Where(i => i["LeagueID"].ToString() == leagueId.ToString(CultureInfo.InvariantCulture))
                                        .Select(
                                            i =>
                                                new Sportbook
                                                {
                                                    AwayId = int.Parse(i["awayID"].ToString()),
                                                    HomeId = int.Parse(i["homeID"].ToString()),
                                                    BetStatus =
                                                        i["betstatus"] != DBNull.Value
                                                            ? i["betstatus"].ToString()
                                                            : string.Empty,
                                                    LeagueId = leagueId,
                                                    MatchId = int.Parse(i["matchid"].ToString()),
                                                    EventStatus = i["eventstatus"].ToString(),
                                                    FinalAwayScore =
                                                        i["finalawayscore"] != DBNull.Value
                                                            ? int.Parse(i["finalawayscore"].ToString())
                                                            : new int?(),
                                                    HtAwayScore =
                                                        i["htawayscore"] != DBNull.Value
                                                            ? int.Parse(i["htawayscore"].ToString())
                                                            : new int?(),
                                                    Color = i.Table.Columns.Contains("lcolor") &&
                                                        i["lcolor"] != DBNull.Value
                                                            ? i["lcolor"].ToString()
                                                            : string.Empty,
                                                    FinalHomeScore =
                                                        i["finalhomescore"] != DBNull.Value
                                                            ? int.Parse(i["finalhomescore"].ToString())
                                                            : new int?(),
                                                    HtHomeScore =
                                                        i["finalhomescore"] != DBNull.Value
                                                            ? int.Parse(i["hthomescore"].ToString())
                                                            : new int?(),
                                                    EventDate = DateTime.Parse(i["eventdate"].ToString()),
                                                    IsNeutral = bool.Parse(i["IsNeutral"].ToString()),
                                                    IsPending = bool.Parse(i["IsPending"].ToString()),
                                                    KickOffTime = DateTime.Parse(i["kickofftime"].ToString()),
                                                    MatchCode = i["matchcode"].ToString(),
                                                    GoalRecord = i["goalrecord"].ToString(),
                                                    ResultValue = string.Empty, // "6-1 *30-40 40-30* *40-0 40-0* *40-0 40-0* *40-0",
                                                    IsHTCal = Convert.ToBoolean(i["IsHTCal"]),
                                                    Calculate = Convert.ToBoolean(i["Calculate"]),
                                                    RefNo = (refNo == -1)
                                                        ? "12"
                                                        : refNo.ToString(CultureInfo.InvariantCulture)
                                                })
                                        .ToList()
                            });
                    }
                }

                return dataParlay;
            }

            return new List<Parlay>();
        }

        #endregion Biding
    }
}