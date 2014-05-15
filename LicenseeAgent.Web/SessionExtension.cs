// --------------------------------------------------------------------------
// <copyright file="SessionExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web
{
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// SessionExtension class.
    /// </summary>
    public static class SessionExtension
    {
        public static bool IsViewSelectMenu(this ICurrentSession currentSession)
        {
            return currentSession.IsAdmin;
        }

        public static bool IsViewIntro(this ICurrentSession currentSession)
        {
            return IsViewSelectMenu(currentSession) && currentSession.AgentInfo == null;
        }

        public static bool IsViewSelectLicensee(this ICurrentSession currentSession)
        {
            return currentSession.IsAdmin && currentSession.LoginType == LoginType.AdminSite;
        }

        public static bool IsViewAccountDropDownMenu(this ICurrentSession currentSession)
        {
            return !(currentSession.LicenseeInfo != null
                || (currentSession.LoginType == LoginType.LRFSite && currentSession.AgentInfo.IsLevel0));
        }

        public static bool IsViewChangePassword(this ICurrentSession currentSession)
        {
            return currentSession.AgentInfo != null
                && !currentSession.AgentInfo.IsLevel0
                ? true
                : false;
        }

        public static bool IsAgentChangePassword(this ICurrentSession currentSession)
        {
            var isAgentChangePassword = false;
            if (currentSession.LoginAccountType == AccountType.Agent)
            {
                var passwordStatus = currentSession.AgentInfo.PasswordStatus;
                isAgentChangePassword = passwordStatus == PasswordStatus.FirstLogin || passwordStatus == PasswordStatus.Expired;
            }

            return isAgentChangePassword;
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

        public static bool IsShowMaintenance(this ICurrentSession currentSession)
        {
            return AppSettings.IsAdminSite
                && currentSession.IsCSDIT
                && AppSettings.TrustedAdmins.Contains(currentSession.LoginUserName);
        }
    }
}