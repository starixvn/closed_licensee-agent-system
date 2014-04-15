// -----------------------------------------------------------------------
// <copyright file="SearchAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SearchAgentCriteria class.
    /// </summary>
    public class SearchAgentCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string UserName { get; set; }

        public int? AccStatus { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SearchAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}