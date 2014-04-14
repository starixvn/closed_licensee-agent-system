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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public int Status { get; set; }

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