// --------------------------------------------------------------------------
// <copyright file="InvalidAgentException.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Exceptions
{
    using System;
    using System.Text;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class InvalidAgentException : Exception
    {
        public InvalidAgentException(AgentInfo agentInfo, string reason)
        {
            AgentInfo = agentInfo;
            Reason = reason;
        }

        public AgentInfo AgentInfo { get; private set; }

        public string Reason { get; private set; }

        public override string Message
        {
            get
            {
                var stringBuilder = new StringBuilder(Reason);
                if (AgentInfo != null)
                {
                    stringBuilder.AppendFormat("\nAgentId: {0}", AgentInfo.AgentId);
                    stringBuilder.AppendFormat("\nLicenseeId: {0}", AgentInfo.LicenseeId);
                    stringBuilder.AppendFormat("\nCurrencyId: {0}", AgentInfo.CurrencyId);
                }

                return stringBuilder.ToString();
            }
        }
    }
}