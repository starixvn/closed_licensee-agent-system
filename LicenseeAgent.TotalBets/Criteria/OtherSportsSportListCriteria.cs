// --------------------------------------------------------------------------
// <copyright file="OtherSportsSportListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Other Sports Sport List Criteria.
    /// </summary>
    public class OtherSportsSportListCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_OtherSports_GetSportList";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}