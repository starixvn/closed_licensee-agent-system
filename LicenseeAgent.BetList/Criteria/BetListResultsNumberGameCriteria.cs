namespace LicenseeAgent.BetList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListResultsNumberGameCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "LAS_Betlist_SelMatchResultNumberGame";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}