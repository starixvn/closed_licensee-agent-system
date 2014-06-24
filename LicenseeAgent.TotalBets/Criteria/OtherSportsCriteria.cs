// --------------------------------------------------------------------------
// <copyright file="OtherSportsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Basketball Criteria.
    /// </summary>
    public class OtherSportsCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string SportList { get; set; }

        public string LeagueList { get; set; }

        public string MatchList { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_OtherSports_MajorBetType";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}