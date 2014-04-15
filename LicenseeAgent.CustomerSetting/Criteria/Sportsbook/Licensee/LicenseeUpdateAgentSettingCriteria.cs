// --------------------------------------------------------------------------
// <copyright file="LicenseeUpdateAgentSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.SportsBook.Licensee
{
    /// <summary>
    /// LicenseeUpdateAgentSettingCriteria class.
    /// </summary>
    public class LicenseeUpdateAgentSettingCriteria : UpdateCustomerCriteria
    {
        public LicenseeUpdateAgentSettingCriteria(UpdateCustomerCriteria updateCustomerCriteria)
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