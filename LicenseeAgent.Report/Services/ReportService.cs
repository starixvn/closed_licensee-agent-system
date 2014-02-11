// ----------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/16">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Services;
    using Criteria;
    using DataAccess;
    using Entities;
    using Fanex.NAP.Framework.Data;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportService : QueryService, IReportService
    {
        #region Fields

        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly IResultsReportDataAccess _resultsReportDataAccess;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService" /> class.
        /// </summary>
        /// <param name="resultsReportDataAccess">Results report data access.</param>
        public ReportService(IResultsReportDataAccess resultsReportDataAccess)
        {
            _resultsReportDataAccess = resultsReportDataAccess ?? new ResultsReportDataAccess();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Sports.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List of sports.</returns>
        public IEnumerable<Sport> GetSports(SportCriteria criteria)
        {
            return GetEntites<Sport>(criteria);
        }

        /// <summary>
        /// Get Leagues.
        /// </summary>
        /// <param name="criteria">Input Criteria.</param>
        /// <returns>List of leagues.</returns>
        public IEnumerable<League> GetLeagues(LeagueCriteria criteria)
        {
            return GetEntites<League>(criteria);
        }

        /// <summary>
        /// Get match results.
        /// </summary>
        /// <param name="criteria">System parlay result criteria.</param>
        /// <returns>List of sports book results.</returns>
        public IList<Sportbook> GetMatchResults(ICriteria criteria)
        {
            var data = _resultsReportDataAccess.GetResults(criteria);
            return GenerateSportsbookResults(data);
        }

        /// <summary>
        /// Get outright results.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>List of outright results.</returns>
        public IList<OutRight> GetOutrightResults(ICriteria criteria)
        {
            var data = _resultsReportDataAccess.GetResults(criteria);
            return GenerateOutrightResults(data);
        }

        public IEnumerable<AgentWinLoss> AgentWinLossReport(ICriteria criteria)
        {
            return GetEntites<AgentWinLoss>(criteria);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Generate Sports book Results.
        /// </summary>
        /// <param name="data">Input data table.</param>
        /// <returns>List of formatted sports book.</returns>
        private static IList<Sportbook> GenerateSportsbookResults(DataTable data)
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
                                            : -1,
                                    HtAwayScore =
                                        i["htawayscore"] != DBNull.Value ? int.Parse(i["htawayscore"].ToString()) : -1,
                                    Color = i.Table.Columns.Contains("lcolor") && i["lcolor"] != DBNull.Value ? i["lcolor"].ToString() : string.Empty,
                                    FinalHomeScore =
                                        i["finalhomescore"] != DBNull.Value
                                            ? int.Parse(i["finalhomescore"].ToString())
                                            : -1,
                                    HtHomeScore =
                                        i["hthomescore"] != DBNull.Value
                                            ? int.Parse(i["hthomescore"].ToString())
                                            : -1,
                                    EventDate = DateTime.Parse(i["eventdate"].ToString()),
                                    IsNeutral = bool.Parse(i["IsNeutral"].ToString()),
                                    IsPending = bool.Parse(i["IsPending"].ToString()),
                                    KickOffTime = DateTime.Parse(i["kickofftime"].ToString()),
                                    MatchCode = i["matchcode"].ToString(),
                                    GoalRecord = i["goalrecord"].ToString(),
                                    ResultValue = "6-1 *30-40 40-30* *40-0 40-0* *40-0 40-0* *40-0",
                                    IsHTCal = Convert.ToBoolean(i["IsHTCal"]),
                                    Calculate = Convert.ToBoolean(i["Calculate"]),
                                    RefNo = "12"
                                })
                        .ToList();
            }

            return new List<Sportbook>();
        }

        /// <summary>
        /// Generate Outright Results.
        /// </summary>
        /// <param name="data">Input data table.</param>
        /// <returns>List of formatted outright.</returns>
        private static IList<OutRight> GenerateOutrightResults(DataTable data)
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

        #endregion
    }
}
