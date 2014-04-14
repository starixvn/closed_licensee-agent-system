// --------------------------------------------------------------------------
// <copyright file="SoccerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Soccer Criteria.
    /// </summary>
    public class SoccerCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string LeagueList { get; set; }

        public string MatchList { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_Soccer_MajorBetType";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}