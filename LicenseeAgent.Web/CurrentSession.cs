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
            : this(adminInfo.UserId, adminInfo.UserName, adminInfo.AccountType, checkinOnUserId, false, language)
        {
            if (adminInfo.AccountType != AccountType.CSDIT)
            {
                throw new InvalidOperationException(string.Format("Invalid account type."));
            }

            AdminInfo = adminInfo;
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
            : this(adminInfo.UserId, adminInfo.UserName, adminInfo.AccountType, checkinOnUserId, true, language)
        {
            if (adminInfo.AccountType != AccountType.CSDIT && adminInfo.AccountType != AccountType.Licensee)
            {
                throw new InvalidOperationException(string.Format("Invalid account type."));
            }

            AdminInfo = adminInfo;
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
            : this(agentInfo.AgentId, agentInfo.UserName, AccountType.Agent, checkinOnUserId, false, language)
        {
            AgentInfo = agentInfo;
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
            internalLoginInfo.IsInternalLRF,
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
            int checkinOnUserId,
            bool isInternalLRF,
            string language)
        {
            LoginUserId = loginUserId;
            LoginUserName = loginUserName;
            LoginAccountType = loginAccountType;
            OnUserId = checkinOnUserId;
            IsInternalLRF = isInternalLRF;
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

        public bool IsInternalLRF { get; private set; }

        /// <summary>
        /// Gets the type of the account who created session.
        /// </summary>
        /// <value>
        /// The type of the account.
        /// </value>
        public AccountType LoginAccountType { get; private set; }

        public bool IsValid
        {
            get
            {
                if (LoginAccountType != AccountType.CSDIT && LoginAccountType != AccountType.Licensee && LoginAccountType != AccountType.Agent)
                {
                    Logger.Error(string.Format("Invalid session - Invalid account type {0}", LoginAccountType));
                    return false;
                }

                if (AppSettings.IsAdminSite)
                {
                    if (LoginAccountType != AccountType.CSDIT)
                    {
                        Logger.Error("Invalid session - Only CSD/IT are allowed to login on admin site");
                        return false;
                    }
                }
                else
                {
                    if (LoginAccountType == AccountType.CSDIT && (AgentInfo == null || AgentInfo.IsLevel0))
                    {
                        Logger.Error("Invalid session - CSD/IT internal login");
                        return false;
                    }

                    if (LoginAccountType == AccountType.Agent && (AgentInfo == null || AgentInfo.IsLevel0))
                    {
                        Logger.Error("Invalid session - Agent level 0 are not allowed to login");
                        return false;
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