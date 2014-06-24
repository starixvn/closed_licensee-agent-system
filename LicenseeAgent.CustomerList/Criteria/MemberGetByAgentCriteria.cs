// -----------------------------------------------------------------------
// <copyright file="MemberGetByAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// MemberGetByAgentCriteria class.
    /// </summary>
    public class MemberGetByAgentCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_MemberGetByAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}