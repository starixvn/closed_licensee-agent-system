// --------------------------------------------------------------------------
// <copyright file="AgentUpdateMemberSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Member
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Criteria information.
    /// </summary>
    public class AgentUpdateMemberSettingCriteria : UpdateCustomerCriteria
    {
        public AgentUpdateMemberSettingCriteria(UpdateCustomerCriteria updateCustomerCriteria)
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