// -----------------------------------------------------------------------
// <copyright file="BetListResultsVirtualRacingCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListResultsVirtualRacingCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public string RaceNo { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "LAS_BetList_SelMatchResultVirtualRacing";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}