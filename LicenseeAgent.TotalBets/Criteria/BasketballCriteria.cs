// --------------------------------------------------------------------------
// <copyright file="BasketballCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.12.25">Created</change>
// </history>

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Basketball Criteria.
    /// </summary>
    public class BasketballCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string LeagueList { get; set; }

        public string MatchList { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_Basketball_MajorBetType";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
