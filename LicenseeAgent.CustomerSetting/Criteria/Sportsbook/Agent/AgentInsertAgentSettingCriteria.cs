// --------------------------------------------------------------------------
// <copyright file="AgentInsertAgentSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.28">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Agent
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentInsertAgentSettingCriteria : InsertCustomerCriteria
    {
        public AgentInsertAgentSettingCriteria(InsertCustomerCriteria criteria)
            : base(criteria)
        {
        }

        public string UserSalt { get; set; }

        public int ParentId { get; set; }

        public int AgentId { get; set; }

        public short MinPTOutput { get; set; }

        public short MaxPTOutput { get; set; }

        public string UserPwd { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentInsAgentAdvanceMode";
        }

        public override bool IsValid()
        {
            return base.IsValid() &&
                ParentId > 0 &&
                !string.IsNullOrEmpty(UserSalt) &&
                !string.IsNullOrEmpty(UserPwd);
        }
    }
}