// --------------------------------------------------------------------------
// <copyright file="AgentGetMemberIndirectSettingForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.VirtualSports
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// AgentGetMemberIndirectSettingForUpdateCriteria class.
    /// </summary>
    public class AgentGetMemberIndirectSettingForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetMemberIndirectForUpdateVirtualSports";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}