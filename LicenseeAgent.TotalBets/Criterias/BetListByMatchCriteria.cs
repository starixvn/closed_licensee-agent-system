// ----------------------------------------------------------------------
// <copyright file="BetListByMatchCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/03/04">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.TotalBets.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Bet list by match Criteria.
    /// </summary>
    public class BetListByMatchCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MatchId { get; set; }

        public int Bettype { get; set; }

        public string Betteam { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Betlist_SelRunningByBettypeMatch";
        }

        public override bool IsValid()
        {
            return (AgentId > 0 && MatchId > 0)
                && (Bettype > 0 && !string.IsNullOrEmpty(Betteam));
        }
    }
}
