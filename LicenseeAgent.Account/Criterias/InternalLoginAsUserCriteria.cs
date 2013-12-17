// -----------------------------------------------------------------------
// <copyright file="InternalLoginAsUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Criterias
{
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InternalLoginAsUserCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public string UserName { get; set; }

        public int AgentId { get; set; }

        public string LicenseeName { get; set; }

        public string CurrencyName { get; set; }

        public string AgentName { get; set; }

        public short NumberOfLevel { get; set; }

        public short AgentLevel { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_InternalLoginAsUser";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0 &&
                CurrencyId > 0;
        }

        /// <summary>
        /// Creates the agent.
        /// </summary>
        /// <returns>Returns agent info object.</returns>
        public AgentInfo CreateAgent()
        {
            return new AgentInfo
            {
                LicenseeId = LicenseeId,
                LicenseeName = LicenseeName,
                CurrencyId = CurrencyId,
                CurrencyName = CurrencyName,
                UserName = AgentName,
                AgentId = AgentId,
                AgentLevel = AgentLevel,
                NumberOfLevel = NumberOfLevel
            };
        }
    }
}