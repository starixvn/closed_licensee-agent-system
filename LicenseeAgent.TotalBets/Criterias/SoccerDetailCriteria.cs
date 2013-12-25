// -----------------------------------------------------------------------
// <copyright file="SoccerDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// SoccerDetailCriteria.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Soccer Detail Criteria.
    /// </summary>
    public class SoccerDetailCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MatchID { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_Soccer_AllBetType";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}