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

        public string Sports { get; set; }

        public string Leagues { get; set; }

        public string Teams { get; set; }

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