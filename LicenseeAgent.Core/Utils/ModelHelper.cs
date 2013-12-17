// --------------------------------------------------------------------------
// <copyright file="ModelHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Utils
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ModelHelper
    {
        public static bool IsAgentLevel0(int agentLevel)
        {
            return agentLevel == 1 ? true : false;
        }

        public static bool IsAgentLevel1(int agentLevel)
        {
            return agentLevel == 2 ? true : false;
        }

        public static bool IsLastAgent(int agentLevel, int numberOfLevel)
        {
            return agentLevel - 1 == numberOfLevel;
        }
    }
}