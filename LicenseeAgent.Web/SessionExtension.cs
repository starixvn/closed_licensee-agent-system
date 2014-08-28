namespace LicenseeAgent.Web
{
    using System;
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
            return !currentSession.IsInternalLRF;
        }

        public static bool IsViewAccountDropDownMenu(this ICurrentSession currentSession)
        {
            return !currentSession.IsInternalLRF;
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
            return currentSession.IsAdmin ? currentSession.LoginUserName : currentSession.AgentInfo.UserName;
        }

        public static bool IsDirectDownLine(this ICurrentSession currentSession, int agentLevel)
        {
            return currentSession.AgentInfo.IsLevel0
                || agentLevel == currentSession.AgentInfo.AgentLevel;
        }

        public static bool IsViewPlayerCommission(this ICurrentSession currentSession)
        {
            return currentSession.AgentInfo != null;
        }

        public static bool IsShowMaintenance(this ICurrentSession currentSession)
        {
            var isTrusted = AppSettings.TrustedAdmins == "*"
                || AppSettings.TrustedAdmins.IndexOf(currentSession.LoginUserName, StringComparison.OrdinalIgnoreCase) != -1;
            return AppSettings.IsAdminSite
                && currentSession.IsCSDIT
                && isTrusted;
        }
    }
}