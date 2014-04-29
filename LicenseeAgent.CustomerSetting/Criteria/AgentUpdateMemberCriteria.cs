// --------------------------------------------------------------------------
// <copyright file="AgentUpdateMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria
{
    /// <summary>
    /// AgentUpdateMemberCriteria class.
    /// </summary>
    public class AgentUpdateMemberCriteria : UpdateCustomerCriteria
    {
        public AgentUpdateMemberCriteria(UpdateCustomerCriteria updateCustomerCriteria)
            : base(updateCustomerCriteria)
        {
        }

        public int CustId { get; set; }

        /// <summary>
        /// Get Setting key.
        /// </summary>
        /// <returns>Current setting key.</returns>
        public override string GetSettingKey()
        {
            return "Las_AgentEditMem";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}