// -----------------------------------------------------------------------
// <copyright file="AgentGetByAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// AgentGetByAgentCriteria class.
    /// </summary>
    public class AgentGetByAgentCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetByAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}