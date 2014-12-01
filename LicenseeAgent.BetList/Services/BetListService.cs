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
        #region Variables

        private readonly IBetListDataAccess _betlistDataAccess;

        #endregion Variables

        #region Constructors

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

        #endregion Constructors

        #region Member

        public BetListResult GetByMember(SelectByMemberCriteria criteria)
        {
            return GetBetList(criteria);
        }

        #endregion Member

        #region Total Bets

        public BetListResult GetTotalBets(TotalBetsCriteria criteria)
        {
            return GetBetList(criteria);
        }

        #endregion Total Bets

        #region Outstanding

        public BetListResult GetOutstanding(MemberOutstandingCriteria criteria)
        {
            return GetBetList(criteria);
        }

        #endregion Outstanding

        #region Outright

        public BetListResult GetOutright(OutrightCriteria criteria)
        {
            return GetBetList(criteria);
        }

        #endregion Outright

        #region Mix Parlay Details

        public BetListResult GetMixParlayDetails(MixParlayDetailsCriteria criteria)
        {
            return GetBetList(criteria);
        }

        #endregion Mix Parlay Details

        #region System Parlay Details

        /// <summary>
        /// Get system parlay result data.
        /// </summary>
        /// <param name="criteria">System parlay result criteria.</param>
        /// <returns>System Parlay Data.</returns>
        public SystemParlayData GetSystemParlayResult(SystemParlayDetailsCriteria criteria)
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

        #endregion System Parlay Details

        #region Results

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
            var criteria = new ResultsNumberGameCriteria
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
            var criteria = new ResultsVirtualRacingCriteria
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
            var criteria = new ResultsSportsBookCriteria
            {
                MatchId = matchId
            };

            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingSportsBook(dataTable);
        }

        #endregion Results

        #region Details

        public IList<TicketDetail> GetDetails(TicketDetailsCriteria criteria)
        {
            return GetEntites<TicketDetail>(criteria);
        }

        #endregion Details

        #region Parlay

        public BetListResult GetParlay(TotalBetsParlayCriteria criteria)
        {
            return GetBetList(criteria);
        }

        public IList<Parlay> GetParlayResults(ResultsParlayCriteria criteria)
        {
            var dataTable = ExecuteDataTable(criteria);
            return ResultBinder.BindingParlay(dataTable, criteria.RefNo);
        }

        #endregion Parlay

        #region Utility

        /// <summary>
        /// Get Bet List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        protected BetListResult GetBetList(ICriteria criteria)
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

        #endregion Utility
    }
}