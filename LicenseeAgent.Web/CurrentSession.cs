namespace LicenseeAgent.Web
{
    using System;
    using LicenseeAgent.Core.Configurations;
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
            : this(adminInfo.UserId, adminInfo.UserName, adminInfo.AccountType, checkinOnUserId, language)
        {
            if (adminInfo.AccountType != AccountType.CSDIT)
            {
                throw new InvalidOperationException(string.Format("Invalid account type."));
            }

            AdminInfo = adminInfo;
            IsInternal = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession" /> class.
        /// CSD/IT or Licensee Admin internal sign-in.
        /// </summary>
        /// <param name="adminInfo">The admin information.</param>
        /// <param name="checkinOnUserId">The check-in on user identifier.</param>
        /// <param name="checkinOnLicenseeId">The check-in on licensee identifier.</param>
        /// <param name="language">The language.</param>
        public CurrentSession(AdminInfo adminInfo, int checkinOnUserId, int checkinOnLicenseeId, string language = LanguageManager.UsLanguageCode)
            : this(adminInfo.UserId, adminInfo.UserName, adminInfo.AccountType, checkinOnUserId, language)
        {
            if (adminInfo.AccountType != AccountType.CSDIT && adminInfo.AccountType != AccountType.Licensee)
            {
                throw new InvalidOperationException(string.Format("Invalid account type."));
            }

            AdminInfo = adminInfo;
            IsInternal = true;
            OnLicenseeId = checkinOnLicenseeId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession"/> class.
        /// Agent direct login.
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <param name="checkinOnUserId">The check-in on user identifier.</param>
        /// <param name="language">The language.</param>
        public CurrentSession(AgentInfo agentInfo, int checkinOnUserId, string language = LanguageManager.UsLanguageCode)
            : this(agentInfo.AgentId, agentInfo.UserName, AccountType.Agent, checkinOnUserId, language)
        {
            AgentInfo = agentInfo;
            IsInternal = false;
            OnLicenseeId = agentInfo.LicenseeId;
            OnValidateAgent(agentInfo);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession" /> class.
        /// Agent internal login.
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <param name="internalLoginInfo">The internal login information.</param>
        public CurrentSession(AgentInfo agentInfo, InternalLoginInfo internalLoginInfo)
            : this(
            internalLoginInfo.LoginUserId,
            internalLoginInfo.LoginUserName,
            internalLoginInfo.LoginAccountType,
            internalLoginInfo.OnLoginUserId,
            internalLoginInfo.Language)
        {
            AgentInfo = agentInfo;
            IsInternal = true;
            OnLicenseeId = internalLoginInfo.LicenseeId;
            OnValidateAgent(agentInfo);
        }

        protected CurrentSession(
            int loginUserId,
            string loginUserName,
            AccountType loginAccountType,
            int checkinOnUserId,
            string language)
        {
            LoginUserId = loginUserId;
            LoginUserName = loginUserName;
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

        public bool IsInternal { get; private set; }

        public bool IsValid
        {
            get
            {
                if (AppSettings.IsAdminSite)
                {
                    // Only CSD/IT to login, not accept internal.
                    if (LoginAccountType != AccountType.CSDIT)
                    {
                        return false;
                    }
                }
                else
                {
                    if (AdminInfo != null)
                    {
                        return false;
                    }

                    if (AgentInfo == null)
                    {
                        if (LoginAccountType == AccountType.Agent)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (AgentInfo.IsLevel0 && (!IsInternal || LoginAccountType != AccountType.Licensee))
                        {
                            return false;
                        }

                        if (!IsInternal && LoginAccountType == AccountType.Licensee)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return AdminInfo != null || AgentInfo.IsLevel0;
            }
        }

        public bool IsCSDIT
        {
            get
            {
                return IsAdmin && LoginAccountType == AccountType.CSDIT;
            }
        }

        public bool IsLicensee
        {
            get
            {
                return IsAdmin && LoginAccountType == AccountType.Licensee;
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