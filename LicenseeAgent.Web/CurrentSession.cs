// --------------------------------------------------------------------------
// <copyright file="CurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web
{
    using System;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    ///  It contains all properties of an authenticated user.
    ///  The implementation is applied when user config is state server mode.
    /// </summary>
    [Serializable]
    public class CurrentSession : ICurrentSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession"/> class.
        /// CSD/IT direct login.
        /// </summary>
        /// <param name="adminInfo">The admin information.</param>
        /// <param name="checkinOnUserId">The check-in on user identifier.</param>
        /// <param name="language">The language.</param>
        public CurrentSession(AdminInfo adminInfo, int checkinOnUserId, string language = LanguageManager.UsLanguageCode)
            : this(adminInfo.UserId, adminInfo.UserName, adminInfo.AccountType, LoginType.AdminSite, checkinOnUserId, language)
        {
            AdminInfo = adminInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession"/> class.
        /// Licensee direct login from LRF.
        /// </summary>ger
        /// <param name="licenseeInfo">The licensee information.</param>
        /// <param name="checkinOnUserId">The check-in on user identifier.</param>
        /// <param name="language">The language.</param>
        public CurrentSession(LicenseeInfo licenseeInfo, int checkinOnUserId, string language = LanguageManager.UsLanguageCode)
            : this(licenseeInfo.UserId, licenseeInfo.UserName, AccountType.Licensee, LoginType.LRFSite, checkinOnUserId, language)
        {
            LicenseeInfo = licenseeInfo;
            OnLicenseeId = licenseeInfo.LicenseeId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession"/> class.
        /// Agent direct login.
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <param name="checkinOnUserId">The check-in on user identifier.</param>
        /// <param name="language">The language.</param>
        public CurrentSession(AgentInfo agentInfo, int checkinOnUserId, string language = LanguageManager.UsLanguageCode)
            : this(agentInfo.AgentId, agentInfo.UserName, AccountType.Agent, LoginType.AgentSite, checkinOnUserId, language)
        {
            AgentInfo = agentInfo;
            OnLicenseeId = agentInfo.LicenseeId;
            OnValidateAgent(agentInfo);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession" /> class.
        /// Agent indirect login.
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <param name="internalLoginInfo">The internal login information.</param>
        public CurrentSession(AgentInfo agentInfo, InternalLoginInfo internalLoginInfo)
            : this(
            internalLoginInfo.LoginUserId,
            internalLoginInfo.LoginUserName,
            internalLoginInfo.LoginAccountType,
            internalLoginInfo.LoginType,
            internalLoginInfo.OnLoginUserId,
            internalLoginInfo.Language)
        {
            AgentInfo = agentInfo;
            OnLicenseeId = internalLoginInfo.LicenseeId;
            OnValidateAgent(agentInfo);
        }

        protected CurrentSession(
            int loginUserId,
            string loginUserName,
            AccountType loginAccountType,
            LoginType loginType,
            int checkinOnUserId,
            string language)
        {
            if (loginAccountType != AccountType.CSD_IT && loginAccountType != AccountType.Licensee && loginAccountType != AccountType.Agent)
            {
                throw new InvalidOperationException(string.Format("Invalid account type {0}", loginAccountType));
            }

            LoginUserId = loginUserId;
            LoginUserName = loginUserName;
            LoginType = loginType;
            LoginAccountType = loginAccountType;
            OnUserId = checkinOnUserId;
            Language = language;
        }

        /// <summary>
        /// Gets the admin account information after login.
        /// </summary>
        /// <value>
        /// The admin information.
        /// </value>
        public AdminInfo AdminInfo { get; private set; }

        /// <summary>
        /// Gets the licensee account information after login.
        /// </summary>
        /// <value>
        /// The licensee information.
        /// </value>
        public LicenseeInfo LicenseeInfo { get; private set; }

        /// <summary>
        /// Gets the agent account information after login or internal login.
        /// </summary>
        /// <value>
        /// The agent information.
        /// </value>
        public AgentInfo AgentInfo { get; private set; }

        public string Language { get; set; }

        public int LoginUserId { get; private set; }

        public string LoginUserName { get; private set; }

        public int OnUserId { get; private set; }

        public int OnLicenseeId { get; private set; }

        /// <summary>
        /// Gets the type of the account who created session.
        /// </summary>
        /// <value>
        /// The type of the account.
        /// </value>
        public AccountType LoginAccountType { get; private set; }

        public LoginType LoginType { get; private set; }

        public bool IsDirectSignIn
        {
            get
            {
                if (AdminInfo != null || LicenseeInfo != null)
                {
                    return true;
                }

                return AgentInfo == null || AgentInfo.AgentId == LoginUserId ? true : false;
            }
        }

        public bool IsValid
        {
            get
            {
                if (AgentInfo != null && AgentInfo.IsLevel0 && LoginType == LoginType.AgentSite)
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return AdminInfo != null || LicenseeInfo != null || AgentInfo.IsLevel0;
            }
        }

        public bool IsCSDIT
        {
            get
            {
                return IsAdmin && LoginAccountType == AccountType.CSD_IT;
            }
        }

        public void UpdateAgent(AgentInfo agentInfo)
        {
            AgentInfo = agentInfo;
            OnLicenseeId = agentInfo.LicenseeId;
            OnValidateAgent(agentInfo);
        }

        /// <summary>
        /// Called when [validate agent].
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Agent is invalid.
        /// or
        /// Licensee is invalid.
        /// or
        /// Level is invalid.
        /// or
        /// Licensee is invalid.
        /// or
        /// Currency is invalid.
        /// </exception>
        protected void OnValidateAgent(AgentInfo agentInfo)
        {
            if (agentInfo == null)
            {
                throw new InvalidOperationException("Agent is null.");
            }

            if (OnLicenseeId != 0 && OnLicenseeId != agentInfo.LicenseeId)
            {
                throw new InvalidAgentException(agentInfo, "Licensee is invalid.");
            }

            if (agentInfo.AgentLevel - 1 > agentInfo.NumberOfLevel)
            {
                throw new InvalidAgentException(agentInfo, "Level is invalid.");
            }

            if (agentInfo.LicenseeId == 0 || string.IsNullOrEmpty(agentInfo.LicenseeName))
            {
                throw new InvalidAgentException(agentInfo, "Licensee is invalid.");
            }

            if (agentInfo.CurrencyId == 0 || string.IsNullOrEmpty(agentInfo.CurrencyName))
            {
                throw new InvalidAgentException(agentInfo, "Currency is invalid.");
            }
        }
    }
}