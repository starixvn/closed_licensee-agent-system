// -----------------------------------------------------------------------
// <copyright file="StatisticCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// StatisticCriteria class.
    /// </summary>
    public class StatisticCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelStatisticsInfo";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}