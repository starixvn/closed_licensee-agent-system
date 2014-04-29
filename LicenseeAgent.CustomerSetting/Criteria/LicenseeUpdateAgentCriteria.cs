// --------------------------------------------------------------------------
// <copyright file="LicenseeUpdateAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria
{
    /// <summary>
    /// LicenseeUpdateAgentCriteria class.
    /// </summary>
    public class LicenseeUpdateAgentCriteria : UpdateCustomerCriteria
    {
        public LicenseeUpdateAgentCriteria(UpdateCustomerCriteria updateCustomerCriteria)
            : base(updateCustomerCriteria)
        {
        }

        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeEditAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}