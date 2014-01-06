// -----------------------------------------------------------------------
// <copyright file="OutrightCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutrightCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string SportList { get; set; }

        public string LeagueList { get; set; }

        public string TeamList { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_Outright_SelBetData";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}