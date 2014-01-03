// -----------------------------------------------------------------------
// <copyright file="OutrightSportCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutrightSportCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_Outright_GetSportList";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}