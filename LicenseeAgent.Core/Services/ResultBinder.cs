// -----------------------------------------------------------------------
// <copyright file="ResultBinder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResultBinder
    {
        /// <summary>
        /// Mapping data to Sport-book DTO.
        /// </summary>
        /// <param name="data">The data table.</param>
        /// <returns>The sport-book data list.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1125:UseShorthandForNullableTypes", Justification = "Reviewed.")]
        public static IList<Sportbook> BindingSportsBook(DataTable data)
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
        /// <param name="data">The data table.</param>
        /// <returns>The NG data list.</returns>
        public static IList<NumberGame> BindingNumberGame(DataTable data)
        {
            if (data != null && data.Rows.Count > 0)
            {
                data.AsEnumerable().OrderBy(i => i["leagueId"]).OrderBy(i => i["matchid"]).OrderBy(i => i["seq"]);
                var dataListNg = new List<NumberGame>();
                foreach (var mtc in data.AsEnumerable())
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
        /// <param name="data">The data table.</param>
        /// <returns>The live casino data list.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1125:UseShorthandForNullableTypes", Justification = "Reviewed.")]
        public static IList<LiveCasino> BindingLiveCasino(DataTable data)
        {
            if (data != null && data.Rows.Count > 0)
            {
                return
                    data.AsEnumerable()
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
        /// <param name="data">The data table.</param>
        /// <returns>The racing data list.</returns>
        public static IList<Racing> BindingVirtualHorseRacing(DataTable data)
        {
            if (data != null && data.Rows.Count > 0)
            {
                var result =
                    data.AsEnumerable()
                        .Select(
                            i =>
                                new Racing
                                {
                                    LeagueId = int.Parse(i["leagueid"].ToString()),
                                    EngineId = i["HorseId"].ToString(),
                                    EngineNo = i["HorseNo"].ToString(),
                                    IsFavor = i["IsFavor"] != DBNull.Value && i["IsFavor"].ToString().Equals("1") ? true : false,
                                    IsSCR = false,
                                    Place = int.Parse(i["place"].ToString()),
                                    PlaceOdds = decimal.Parse(i["placeodds"].ToString()),
                                    WinOdds = decimal.Parse(i["winodds"].ToString()),
                                    RaceName = 0,
                                    KickOffTime = i["eventtime"] != DBNull.Value ? DateTime.Parse(i["eventtime"].ToString()) : DateTime.MinValue,
                                    Favourite = i["IsFavor"] != DBNull.Value ? i["IsFavor"].ToString() : string.Empty,
                                    SCROfMatch = string.Empty,
                                    TFOfMatch = string.Empty
                                })
                        .ToList();

                result.ForEach(x =>
                {
                    x.RowCount = result.Count(y => y.KickOffTime == x.KickOffTime);
                    x.WinOdds = x.Place == 1 ? x.WinOdds : 0;
                });
                return result;
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
        public static IList<Parlay> BindingParlay(DataTable data, long refNo)
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

        /// <summary>
        /// Get out right data.
        /// </summary>
        /// <param name="data">The data table.</param>
        /// <returns>
        /// The out right data list.
        /// </returns>
        public static IList<OutRight> BindingOutRight(DataTable data)
        {
            if (data != null && data.Rows.Count > 0)
            {
                return
                    data.AsEnumerable()
                        .Select(
                            i =>
                                new OutRight
                                {
                                    EventDate = DateTime.Parse(i["eventdate"].ToString()),
                                    SportType = int.Parse(i["sporttype"].ToString()),
                                    LeagueId = int.Parse(i["leagueid"].ToString()),
                                    TeamId = int.Parse(i["teamid"].ToString()),
                                    Status = i["status"].ToString()
                                })
                        .ToList();
            }

            return new List<OutRight>();
        }
    }
}