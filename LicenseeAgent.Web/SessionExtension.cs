// --------------------------------------------------------------------------
// <copyright file="SessionExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class SessionExtension
    {
        public static bool IsViewSelectMenu(this ICurrentSession currentSession)
        {
            return (currentSession.AgentInfo == null
                || (currentSession.AgentInfo != null && currentSession.AgentInfo.IsLevel0))
                ? true
                : false;
        }

        public static bool IsViewChangePassword(this ICurrentSession currentSession)
        {
            return currentSession.AgentInfo != null
                && !currentSession.AgentInfo.IsLevel0
                ? true
                : false;
        }

        public static string DisplayName(this ICurrentSession currentSession)
        {
            if (currentSession.AgentInfo != null && !currentSession.AgentInfo.IsLevel0)
            {
                return currentSession.AgentInfo.UserName;
            }

            if (currentSession.AdminInfo != null)
            {
                return currentSession.AdminInfo.UserName;
            }

            if (currentSession.LicenseeInfo != null)
            {
                return currentSession.LicenseeInfo.UserName;
            }

            return currentSession.AgentInfo.UserName;
        }

        public static bool IsDirectDownline(this ICurrentSession currentSession, int agentLevel)
        {
            return currentSession.AgentInfo.IsLevel0
                || agentLevel == currentSession.AgentInfo.AgentLevel;
        }
    }
}
