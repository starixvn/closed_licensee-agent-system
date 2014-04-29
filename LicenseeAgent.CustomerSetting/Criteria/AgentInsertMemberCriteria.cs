// --------------------------------------------------------------------------
// <copyright file="AgentInsertMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria
{
    /// <summary>
    /// AgentInsertMemberCriteria class.
    /// </summary>
    public class AgentInsertMemberCriteria : InsertCustomerCriteria
    {
        public AgentInsertMemberCriteria(InsertCustomerCriteria criteria)
            : base(criteria)
        {
        }

        public int ParentId { get; set; }

        public string UGroup { get; set; }

        public int CustId { get; set; }

        public short MinPTOutput { get; set; }

        public short MaxPTOutput { get; set; }

        public string IBCUserName { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentInsertMember";
        }

        public override bool IsValid()
        {
            return base.IsValid() &&
                ParentId > 0 &&
                !string.IsNullOrEmpty(UGroup);
        }
    }
}