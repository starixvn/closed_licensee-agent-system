// --------------------------------------------------------------------------
// <copyright file="AgentGetAgentSettingForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.VirtualSports
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentGetAgentSettingForUpdateCriteria class.
    /// </summary>
    public class AgentGetAgentSettingForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetAgentVirtualSportsSettingForUpdate";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}