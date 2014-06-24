// -----------------------------------------------------------------------
// <copyright file="OutrightSportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// OutrightSportCriteria class.
    /// </summary>
    public class OutrightSportCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_Outright_GetSportList";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}