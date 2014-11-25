namespace LicenseeAgent.Report.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Result.Core.DTO;
    using LicenseeAgent.Report.Criteria.Results;
    using LicenseeAgent.Report.Entities.Results;

    public interface IResultsService
    {
        IList<SportEntity> GetSports(GetSportCriteria criteria);

        IList<LeagueEntity> GetLeagues(GetLeagueCriteria criteria);

        IList<Sportsbook> GetMatchResultsSportsBook(MatchResultsSportsBookCriteria criteria);

        IList<NumberGame> GetMatchResultsNumberGame(MatchResultsNumberGameCriteria criteria);

        IList<Racing> GetMatchResultsVirtualHorseRacing(MatchResultsVirtualHorseRacingCriteria criteria);

        IList<OutRight> GetOutrightResults(OutrightResultsReportCriteria criteria);
    }
}