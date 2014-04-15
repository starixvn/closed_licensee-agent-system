// --------------------------------------------------------------------------
// <copyright file="AgentUpdateAgentSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.SportsBook.Agent
{
    /// <summary>
    /// Criteria information.
    /// </summary>
    public class AgentUpdateAgentSettingCriteria : UpdateCustomerCriteria
    {
        public AgentUpdateAgentSettingCriteria(UpdateCustomerCriteria criteria)
            : base(criteria)
        {
        }

        public int AgentId { get; set; }

        /// <summary>
        /// Get Setting key.
        /// </summary>
        /// <returns>Current setting key.</returns>
        public override string GetSettingKey()
        {
            return "Las_AgentEditAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}