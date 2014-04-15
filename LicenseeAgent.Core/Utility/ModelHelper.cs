// --------------------------------------------------------------------------
// <copyright file="ModelHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    /// <summary>
    /// ModelHelper class.
    /// </summary>
    public static class ModelHelper
    {
        public static bool IsAgentLevel0(int agentLevel)
        {
            return agentLevel == 1;
        }

        public static bool IsAgentLevel1(int agentLevel)
        {
            return agentLevel == 2;
        }

        public static bool IsLastAgent(int agentLevel, int numberOfLevel)
        {
            return agentLevel - 1 == numberOfLevel;
        }
    }
}