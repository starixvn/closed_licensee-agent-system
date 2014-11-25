namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using Fanex.Result.Core.DTO;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Report.Criteria.Results;
    using LicenseeAgent.Report.Entities.Results;

    /// <summary>
    /// ReportService class.
    /// </summary>
    public class ResultsService : QueryService, IResultsService
    {
        public IList<SportEntity> GetSports(GetSportCriteria criteria)
        {
            return GetEntites<SportEntity>(criteria);
        }

        public IList<LeagueEntity> GetLeagues(GetLeagueCriteria criteria)
        {
            return GetEntites<LeagueEntity>(criteria);
        }

        public IList<Sportsbook> GetMatchResultsSportsBook(MatchResultsSportsBookCriteria criteria)
        {
            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingSportsBook(dataTable);
        }

        public IList<NumberGame> GetMatchResultsNumberGame(MatchResultsNumberGameCriteria criteria)
        {
            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingNumberGame(dataTable);
        }

        public IList<Racing> GetMatchResultsVirtualHorseRacing(MatchResultsVirtualHorseRacingCriteria criteria)
        {
            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingVirtualHorseRacing(dataTable);
        }

        public IList<OutRight> GetOutrightResults(OutrightResultsReportCriteria criteria)
        {
            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingOutRight(dataTable);
        }
    }
}