namespace LicenseeAgent.BetList.DataAccess
{
    using Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using LicenseeAgent.BetList.Criteria;

    /// <summary>
    /// Interface Account.
    /// </summary>
    public interface IBetListDataAccess
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
        /// Get Parlay List.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Ticket results.</returns>
        BetListResult GetParlayList(ICriteria criteria);
    }
}