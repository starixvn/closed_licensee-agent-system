// --------------------------------------------------------------------------
// <copyright file="EditinfoAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.1.20">Created</change>
// </history>

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Edit info Agent Criteria.
    /// </summary>
    public class EditinfoAgentCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public int AgentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Editinfo_Agent";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}