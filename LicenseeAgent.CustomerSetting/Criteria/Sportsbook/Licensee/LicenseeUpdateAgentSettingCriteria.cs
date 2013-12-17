// --------------------------------------------------------------------------
// <copyright file="LicenseeUpdateAgentSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2013.12.06">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Licensee
{
    /// <summary>
    /// TODO: Update summary.
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