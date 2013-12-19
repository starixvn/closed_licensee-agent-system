// --------------------------------------------------------------------------
// <copyright file="SoccerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Total Bets Soccer Criteria.
    /// </summary>
    public class SoccerCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetByAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}