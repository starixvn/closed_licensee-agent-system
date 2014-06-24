// ----------------------------------------------------------------------
// <copyright file="BetListOutrightCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Bet list by match Criteria.
    /// </summary>
    public class BetListOutrightCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int LeagueId { get; set; }

        public int TeamId { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Betlist_SelRunningOutrightTickets";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && LeagueId > 0 && TeamId > 0;
        }
    }
}