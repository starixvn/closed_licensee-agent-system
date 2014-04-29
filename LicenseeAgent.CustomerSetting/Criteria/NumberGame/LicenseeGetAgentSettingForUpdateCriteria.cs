// --------------------------------------------------------------------------
// <copyright file="LicenseeGetAgentSettingForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.NumberGame
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// LicenseeGetAgentSettingForUpdateCriteria class.
    /// </summary>
    public class LicenseeGetAgentSettingForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeGetAgentNumberGameSettingForUpdate";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}