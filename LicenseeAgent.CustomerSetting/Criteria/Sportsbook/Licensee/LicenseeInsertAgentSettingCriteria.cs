// --------------------------------------------------------------------------
// <copyright file="LicenseeInsertAgentSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.30">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Licensee
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseeInsertAgentSettingCriteria : InsertCustomerCriteria
    {
        public LicenseeInsertAgentSettingCriteria(InsertCustomerCriteria criteria)
            : base(criteria)
        {
        }

        public string UserSalt { get; set; }

        public int AgentL0Id { get; set; }

        public int AgentId { get; set; }

        public string UserPwd { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeInsAgentAdvanceMode";
        }

        public override bool IsValid()
        {
            return base.IsValid() &&
                AgentL0Id > 0 &&
                !string.IsNullOrEmpty(UserSalt) &&
                !string.IsNullOrEmpty(UserPwd);
        }
    }
}