namespace LicenseeAgent.BetList.Services
{
    using System;
    using System.Collections.Generic;
    using Core.Services;
    using DataAccess;
    using Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
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
            return GetEntites<RawBetListDetails>(criteria);
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

            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingNumberGame(dataTable);
        }

        /// <summary>
        /// Gets the virtual racing results.
        /// </summary>
        /// <param name="matchId">The match identifier.</param>
        /// <param name="raceNo">The race no.</param>
        /// <returns>Returns list of Racing item.</returns>
        public IList<Racing> GetVirtualRacingResults(int matchId, string raceNo)
        {
            var criteria = new BetListResultsVirtualRacingCriteria
            {
                MatchId = matchId,
                RaceNo = raceNo
            };

            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingVirtualHorseRacing(dataTable);
        }

        /// <summary>
        /// Gets the sports book results.
        /// </summary>
        /// <param name="matchId">The match identifier.</param>
        /// <returns>Return number game result.</returns>
        /// <exception cref="System.InvalidOperationException">Invalid criteria.</exception>
        /// <exception cref="CriteriaException"></exception>
        public IList<Sportsbook> GetSportsBookResults(int matchId)
        {
            var criteria = new BetListResultsSportsBookCriteria
            {
                MatchId = matchId
            };

            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingSportsBook(dataTable);
        }

        /// <summary>
        /// Get Parlay Results.
        /// </summary>
        /// <param name="criteria">Bet List Results Parlay Criteria.</param>
        /// <returns>The parlay list.</returns>
        public IList<Parlay> GetParlayResults(BetListResultsParlayCriteria criteria)
        {
            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingParlay(dataTable, criteria.RefNo);
        }
    }
}