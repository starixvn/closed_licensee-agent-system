namespace LicenseeAgent.Report.Services
{
    using System;
    using System.Collections.Generic;
    using Core.Services;
    using Criteria;
    using DataAccess;
    using Entities;
    using Fanex.Data.Repository;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// ReportService class.
    /// </summary>
    public class ReportService : QueryService, IReportService
    {
        #region Fields

        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly IResultsReportDataAccess _resultsReportDataAccess;

        #endregion Fields

        #region Constructor

        public ReportService()
            : this(null)
        {
        }

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
        public IList<Sport> GetSports(SportCriteria criteria)
        {
            return GetEntites<Sport>(criteria);
        }

        /// <summary>
        /// Get Leagues.
        /// </summary>
        /// <param name="criteria">Input Criteria.</param>
        /// <returns>List of leagues.</returns>
        public IList<League> GetLeagues(LeagueCriteria criteria)
        {
            return GetEntites<League>(criteria);
        }

        /// <summary>
        /// Get match results.
        /// </summary>
        /// <param name="eventDate">The event date.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="leagueId">The league identifier.</param>
        /// <param name="isShowAll">If set to <c>true</c> [is show all].</param>
        /// <returns>
        /// List of sports book results.
        /// </returns>
        public IList<Sportbook> GetMatchResults(DateTime eventDate, int sportType, int leagueId, bool isShowAll)
        {
            var criteria = new MatchResultsReportCriteria
            {
                CustId = 0,
                EventDate = eventDate,
                LeagueId = leagueId,
                SportType = sportType,
                IsShowAll = isShowAll
            };

            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingSportsBook(dataTable);
        }

        /// <summary>
        /// Gets the match results number game.
        /// </summary>
        /// <param name="eventDate">The event date.</param>
        /// <param name="leagueId">The league identifier.</param>
        /// <param name="isShowAll">If set to <c>true</c> [is show all].</param>
        /// <returns>List of number game results.</returns>
        public IList<NumberGame> GetMatchResultsNumberGame(DateTime eventDate, int leagueId, bool isShowAll)
        {
            var criteria = new MatchResultsNumberGameCriteria
            {
                EventDate = eventDate,
                LeagueId = leagueId,
                IsShowAll = isShowAll
            };

            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingNumberGame(dataTable);
        }

        /// <summary>
        /// Gets the match results virtual horse racing.
        /// </summary>
        /// <param name="eventDate">The event date.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="leagueId">The league identifier.</param>
        /// <param name="isShowAll">If set to <c>true</c> [is show all].</param>
        /// <returns>Returns list of Virtual Horse Racing.</returns>
        public IList<Racing> GetMatchResultsVirtualHorseRacing(DateTime eventDate, int sportType, int leagueId, bool isShowAll)
        {
            var criteria = new MatchResultsVirtualHorseRacingCriteria
            {
                EventDate = eventDate,
                SportType = sportType,
                LeagueId = leagueId,
                IsShowAll = isShowAll
            };

            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingVirtualHorseRacing(dataTable);
        }

        /// <summary>
        /// Get outright results.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>List of outright results.</returns>
        public IList<OutRight> GetOutrightResults(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                var data = _resultsReportDataAccess.GetResults(criteria);
                return ResultBinder.BindingOutRight(data);
            }

            return new List<OutRight>();
        }

        /// <summary>
        /// Get outstanding results.
        /// </summary>
        /// <param name="criteria">Outstanding Criteria.</param>
        /// <returns>List of agent outstanding results.</returns>
        public IList<AgentOutstanding> GetOutstandings(ICriteria criteria)
        {
            return GetEntites<AgentOutstanding>(criteria);
        }

        public IList<AgentWinLoss> AgentWinLossReport(ICriteria criteria)
        {
            return GetEntites<AgentWinLoss>(criteria);
        }

        #endregion Methods
    }
}