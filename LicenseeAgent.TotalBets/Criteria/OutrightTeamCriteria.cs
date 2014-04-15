// -----------------------------------------------------------------------
// <copyright file="OutrightTeamCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// OutrightTeamCriteria class.
    /// </summary>
    public class OutrightTeamCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string SportList { get; set; }

        public string LeagueList { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_Outright_GetTeamList";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}