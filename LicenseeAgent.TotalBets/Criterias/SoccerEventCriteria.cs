// -----------------------------------------------------------------------
// <copyright file="SoccerEventCriteria.cs" company="Nexcel Solutions Vietnam">
// SoccerEventCriteria.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Soccer Event Criteria.
    /// </summary>
    public class EventCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int SportType { get; set; }

        public string LeagueList { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_GetMatchBySportType";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}