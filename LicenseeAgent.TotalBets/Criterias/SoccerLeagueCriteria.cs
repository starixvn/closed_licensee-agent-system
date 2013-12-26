// --------------------------------------------------------------------------
// <copyright file="SoccerLeagueCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Soccer League Criteria.
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