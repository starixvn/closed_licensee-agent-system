// -----------------------------------------------------------------------
// <copyright file="OutrightLeagueCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutrightLeagueCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_Outright_GetLeagueList";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}