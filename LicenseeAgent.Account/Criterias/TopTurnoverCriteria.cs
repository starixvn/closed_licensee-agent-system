// -----------------------------------------------------------------------
// <copyright file="TopTurnoverCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TopTurnoverCriteria : CriteriaBase
    {
        public int AgentID { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelTopTurnoverMembers";
        }

        public override bool IsValid()
        {
            return AgentID > 0;
        }
    }
}