// --------------------------------------------------------------------------
// <copyright file="LicenseeGetAgentSettingForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.SportsBook
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LicenseeGetAgentSettingForUpdateCriteria class.
    /// </summary>
    public class LicenseeGetAgentSettingForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeGetAgentSportsBookSettingForUpdate";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}