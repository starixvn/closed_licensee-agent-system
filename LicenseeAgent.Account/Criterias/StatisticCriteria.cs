// -----------------------------------------------------------------------
// <copyright file="StatisticCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StatisticCriteria : CriteriaBase
    {
        public int AgentID { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelStatisticsInfo";
        }

        public override bool IsValid()
        {
            return AgentID > 0;
        }
    }
}