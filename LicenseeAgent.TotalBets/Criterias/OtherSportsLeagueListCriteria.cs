// --------------------------------------------------------------------------
// <copyright file="OtherSportsLeagueListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.12.31">Created</change>
// </history>

namespace LicenseeAgent.TotalBets.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Other Sports League List Criteria.
    /// </summary>
    public class OtherSportsLeagueListCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string SportList { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_OtherSports_GetLeagueList";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
