// --------------------------------------------------------------------------
// <copyright file="AgentGetAgentSettingForInsertCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.SportsBook
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Agent setting for insert new agent.
    /// </summary>
    public class AgentGetAgentSettingForInsertCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MsgId { get; set; }

        public decimal MaxCredit { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSportsBookSettingSel";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}