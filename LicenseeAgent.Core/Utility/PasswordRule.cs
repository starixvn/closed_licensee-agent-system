// --------------------------------------------------------------------------
// <copyright file="PasswordRule.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Password rules: Password must be at least 8 characters (maximum = 24 characters)
    /// without white space and contain at least 3 of the following:
    /// Uppercase letter [A-Z].
    /// Lowercase leader [a-z].
    /// Numeric [0-9].
    /// Special character (!,@,#,etc..).
    /// </summary>
    public static class PasswordRule
    {
        // Source: http://regexlib.com/REDetails.aspx?regexp_id=887
        // Source: http://regexlib.com/REDetails.aspx?regexp_id=2983
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
               "StyleCop.CSharp.Nexcel.NexcelCustomRules",
               "SP2100:CodeLineMustNotBeLongerThan",
               Justification = "Reviewed. Suppression is OK here.")]
        public const string PasswordPattern = @"(?=^.{8,24}$)(?!.*\s)(?=^[\x00-\x7F]+$)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*";

        /// <summary>
        /// The password expired total days.
        /// </summary>
        public const int PasswordExpiredTotalDays = 60;

        public static PasswordStatus GetPasswordStatus(AgentInfo agentInfo)
        {
            return GetPasswordStatus(agentInfo, null);
        }

        /// <summary>
        /// Gets the password status.
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <param name="lastLoginTime">The last login time.</param>
        /// <returns>
        /// Returns password status.
        /// </returns>
        public static PasswordStatus GetPasswordStatus(AgentInfo agentInfo, DateTime? lastLoginTime)
        {
            if (agentInfo.IsFirstLogin)
            {
                return PasswordStatus.FirstLogin;
            }

            var today = DateTime.Today;
            if (today > agentInfo.PwdExpiry)
            {
                return PasswordStatus.Expired;
            }
            else
            {
                if (lastLoginTime != null)
                {
                    // User will be notified 3 times: 2 weeks, 1 week, 1 day in advance of password expiration date.
                    var lastLoginTimeValue = lastLoginTime.Value;
                    var expiredTotalDays = agentInfo.PwdExpiry.Subtract(today).TotalDays;
                    var loginTotalDays = agentInfo.PwdExpiry.Subtract(lastLoginTimeValue).TotalDays;
                    if (expiredTotalDays <= 14)
                    {
                        if (expiredTotalDays > 7)
                        {
                            if (loginTotalDays > 14)
                            {
                                return PasswordStatus.Alert;
                            }
                        }
                        else if (expiredTotalDays > 1)
                        {
                            if (loginTotalDays > 7)
                            {
                                return PasswordStatus.Alert;
                            }
                        }
                        else
                        {
                            if (loginTotalDays > 1)
                            {
                                return PasswordStatus.Alert;
                            }
                        }
                    }
                }
            }

            return PasswordStatus.Normal;
        }

        public static string GetStatusMessage(AgentInfo agentInfo)
        {
            var passwordStatus = GetPasswordStatus(agentInfo);
            return GetStatusMessage(passwordStatus, agentInfo.PwdExpiry);
        }

        /// <summary>
        /// Gets the status message.
        /// </summary>
        /// <param name="passwordStatus">The password status.</param>
        /// <param name="pwdExpiredDate">The password expired date.</param>
        /// <returns>Returns message.</returns>
        public static string GetStatusMessage(PasswordStatus passwordStatus, DateTime pwdExpiredDate)
        {
            var message = string.Empty;
            switch (passwordStatus)
            {
                case PasswordStatus.FirstLogin:
                    message = Las.PasswordFirstUse;
                    break;

                case PasswordStatus.Expired:
                    message = string.Format(Las.PasswordExpired, pwdExpiredDate.ToShortDateString());
                    break;

                case PasswordStatus.Alert:
                    message = string.Format(Las.PasswordWillExpired, pwdExpiredDate.ToShortDateString());
                    break;

                default:
                    break;
            }

            return message;
        }
    }
}