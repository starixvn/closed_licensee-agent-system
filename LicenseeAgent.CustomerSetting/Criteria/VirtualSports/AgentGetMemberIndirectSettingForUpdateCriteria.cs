// --------------------------------------------------------------------------
// <copyright file="AgentGetMemberIndirectSettingForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.VirtualSports
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentGetMemberIndirectSettingForUpdateCriteria class.
    /// </summary>
    public class AgentGetMemberIndirectSettingForUpdateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int AgentId { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetMemberIndirectForUpdateVirtualSports";
        }

        public override bool IsValid()
        {
            return CustId > 0 && AgentId > 0;
        }
    }
}