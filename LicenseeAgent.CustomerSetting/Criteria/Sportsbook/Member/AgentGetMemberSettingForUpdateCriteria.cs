// --------------------------------------------------------------------------
// <copyright file="AgentGetMemberSettingForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>
namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Member
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Agent Position Taking criteria.
    /// </summary>
    public class AgentGetMemberSettingForUpdateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Las_AgentGetMemberSBSettingForUpdate";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}