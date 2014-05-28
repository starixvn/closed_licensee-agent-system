// -----------------------------------------------------------------------
// <copyright file="StatisticCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// StatisticCriteria class.
    /// </summary>
    public class StatisticCriteria : CriteriaBase
    {
        public int AgentID { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelStatisticsInfo";
        }

        public override bool IsValid()
        {
            return AgentID > 0;
        }
    }
}