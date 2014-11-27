namespace LicenseeAgent.BetList.Services
{
    using System.Collections.Generic;
    using Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Fanex.Result.Core.DTO;
    using LicenseeAgent.BetList.Criteria;

    public interface IBetListService
    {
        /// <summary>
        /// Get Bet List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        BetListResult GetBetList(ICriteria criteria);

        /// <summary>
        /// Get Bet List Parlay.
        /// </summary>
        /// <param name="criteria">Mix Parlay Details Criteria.</param>
        /// <returns>Ticket results.</returns>
        BetListResult GetMixParlayDetails(MixParlayDetailsCriteria criteria);

        /// <summary>
        /// Get system parlay result data.
        /// </summary>
        /// <param name="criteria">System parlay result criteria.</param>
        /// <returns>System Parlay Data.</returns>
        SystemParlayData GetSystemParlayResult(SystemParlayDetailsCriteria criteria);

        /// <summary>
        /// Get Bet List Details.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>A list of bet list details.</returns>
        IList<TicketDetail> GetBetListDetails(TicketDetailsCriteria criteria);

        /// <summary>
        /// Get Parlay List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>
        /// Ticket results.
        /// </returns>
        BetListResult GetParlayList(ICriteria criteria);

        /// <summary>
        /// Get Sports book Results.
        /// </summary>
        /// <param name="matchId">The match identifier.</param>
        /// <returns>
        /// The list of matches.
        /// </returns>
        IList<NumberGame> GetNumberGameResults(int matchId);

        IList<Racing> GetVirtualRacingResults(int matchId, string raceNo);

        IList<Sportsbook> GetSportsBookResults(int matchId);

        /// <summary>
        /// Get Parlay Results.
        /// </summary>
        /// <param name="criteria">Bet List Results Parlay Criteria.</param>
        /// <returns>The parlay list.</returns>
        IList<Parlay> GetParlayResults(ResultsParlayCriteria criteria);
    }
}