// --------------------------------------------------------------------------
// <copyright file="AgentInsertMemberSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.30">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Member
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentInsertMemberSettingCriteria : InsertCustomerCriteria
    {
        public AgentInsertMemberSettingCriteria(InsertCustomerCriteria criteria)
            : base(criteria)
        {
        }

        public int ParentId { get; set; }

        public string UGroup { get; set; }

        public int CustId { get; set; }

        public short MinPTOutput { get; set; }

        public short MaxPTOutput { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentInsMemAdvanceMode";
        }

        public override bool IsValid()
        {
            return base.IsValid() &&
                ParentId > 0 &&
                !string.IsNullOrEmpty(UGroup);
        }
    }
}