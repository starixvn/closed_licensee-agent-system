// --------------------------------------------------------------------------
// <copyright file="AgentGetMemberSettingForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.VirtualSports
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// AgentGetMemberSettingForUpdateCriteria class.
    /// </summary>
    public class AgentGetMemberSettingForUpdateCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetMemberVirtualSportsSettingForUpdate";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}