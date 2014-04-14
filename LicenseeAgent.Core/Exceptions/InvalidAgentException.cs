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

    [Serializable]
    public class InvalidAgentException : Exception
    {
        public InvalidAgentException()
        {
        }

        public InvalidAgentException(AgentInfo agentInfo)
        {
            AgentInfo = agentInfo;
        }

        public InvalidAgentException(AgentInfo agentInfo, string message)
            : base(message)
        {
            AgentInfo = agentInfo;
        }

        public AgentInfo AgentInfo { get; private set; }

        public override string Message
        {
            get
            {
                var stringBuilder = new StringBuilder(base.Message);
                if (AgentInfo != null)
                {
                    stringBuilder.AppendFormat("\nLicenseeId: {0}", AgentInfo.LicenseeId);
                    stringBuilder.AppendFormat("\nCurrencyId: {0}", AgentInfo.CurrencyId);
                    stringBuilder.AppendFormat("\nAgentId: {0}", AgentInfo.AgentId);
                    stringBuilder.AppendFormat("\nAgentName: {0}", AgentInfo.UserName);
                    stringBuilder.AppendFormat("\nAgentLevel: {0}", AgentInfo.AgentLevel);
                    stringBuilder.AppendFormat("\nNumberOfLevel: {0}", AgentInfo.NumberOfLevel);
                }

                return stringBuilder.ToString();
            }
        }
    }
}