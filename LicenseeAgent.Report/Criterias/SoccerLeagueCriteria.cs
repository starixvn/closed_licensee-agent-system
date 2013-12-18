// -----------------------------------------------------------------------
// <copyright file="SoccerLeagueCriteria.cs" company="">
// SoccerLeagueCriteria.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Report.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SoccerLeagueCriteria.
    /// </summary>
    public class SoccerLeagueCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int SportType { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_GetLeagueBySportType";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}