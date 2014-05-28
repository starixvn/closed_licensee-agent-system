// -----------------------------------------------------------------------
// <copyright file="TopWinningCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TopWinningCriteria class.
    /// </summary>
    public class TopWinningCriteria : CriteriaBase
    {
        public int AgentID { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelTopWinningMembers";
        }

        public override bool IsValid()
        {
            return AgentID > 0;
        }
    }
}