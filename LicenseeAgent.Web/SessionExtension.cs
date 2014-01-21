// --------------------------------------------------------------------------
// <copyright file="SessionExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web
{
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class SessionExtension
    {
        public static bool IsViewSelectMenu(this ICurrentSession currentSession)
        {
            return currentSession.IsAdmin;
        }

        public static bool IsViewSelectLicensee(this ICurrentSession currentSession)
        {
            return currentSession.IsAdmin && currentSession.LoginAccountType == AccountType.CSD_IT;
        }

        public static bool IsViewAccountDropDownMenu(this ICurrentSession currentSession)
        {
            return !(currentSession.LicenseeInfo != null
                || (currentSession.LoginAccountType == AccountType.Licensee && currentSession.AgentInfo.IsLevel0));
        }

        public static bool IsViewChangePassword(this ICurrentSession currentSession)
        {
            return currentSession.AgentInfo != null
                && !currentSession.AgentInfo.IsLevel0
                ? true
                : false;
        }

        /// <summary>
        /// Displays the name.
        /// </summary>
        /// <param name="currentSession">The current session.</param>
        /// <returns>Returns display name on home page.</returns>
        public static string DisplayName(this ICurrentSession currentSession)
        {
            return currentSession.IsAdmin ? currentSession.LoginUserName : currentSession.AgentInfo.AgentName;
        }

        public static bool IsDirectDownline(this ICurrentSession currentSession, int agentLevel)
        {
            return currentSession.AgentInfo.IsLevel0
                || agentLevel == currentSession.AgentInfo.AgentLevel;
        }
    }
}