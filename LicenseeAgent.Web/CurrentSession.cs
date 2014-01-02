// --------------------------------------------------------------------------
// <copyright file="CurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web
{
    using System;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    ///  It contains all properties of an authenticated user.
    ///  The implementation is applied when user config is state server mode.
    /// </summary>
    [Serializable]
    public class CurrentSession : ICurrentSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession"/> class.
        /// Admin direct login.
        /// </summary>
        /// <param name="adminInfo">The admin information.</param>
        /// <param name="checkinOnUserId">The check-in on user identifier.</param>
        /// <param name="language">The language.</param>
        public CurrentSession(AdminInfo adminInfo, int checkinOnUserId, string language = "en-US")
            : this(adminInfo.UserId, checkinOnUserId, language)
        {
            AdminInfo = adminInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession"/> class.
        /// Licensee direct login from LRF.
        /// </summary>
        /// <param name="licenseeInfo">The licensee information.</param>
        /// <param name="checkinOnUserId">The check-in on user identifier.</param>
        /// <param name="language">The language.</param>
        public CurrentSession(LicenseeInfo licenseeInfo, int checkinOnUserId, string language = "en-US")
            : this(licenseeInfo.UserId, checkinOnUserId, language)
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
        public CurrentSession(AgentInfo agentInfo, int checkinOnUserId, string language = "en-US")
            : this(agentInfo.AgentId, checkinOnUserId, language)
        {
            AgentInfo = agentInfo;
            OnLicenseeId = agentInfo.LicenseeId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession" /> class.
        /// Agent indirect login.
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <param name="internalLoginInfo">The internal login information.</param>
        public CurrentSession(AgentInfo agentInfo, InternalLoginInfo internalLoginInfo)
            : this(internalLoginInfo.UserId, internalLoginInfo.OnUserId, internalLoginInfo.Language)
        {
            AgentInfo = agentInfo;
            OnLicenseeId = internalLoginInfo.LicenseeId;
        }

        protected CurrentSession(int loginUserId, int checkinOnUserId, string language)
        {
            LoginUserId = loginUserId;
            OnUserId = checkinOnUserId;
            Language = language;
        }

        public AdminInfo AdminInfo { get; private set; }

        public LicenseeInfo LicenseeInfo { get; private set; }

        public AgentInfo AgentInfo { get; set; }

        public string Language { get; set; }

        public int LoginUserId { get; private set; }

        public int OnUserId { get; private set; }

        public int OnLicenseeId { get; private set; }

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
                if (AdminInfo == null && LicenseeInfo == null && AgentInfo == null)
                {
                    return false;
                }

                if (AdminInfo != null && !AppSettings.IsAdminSite)
                {
                    return false;
                }

                if (LicenseeInfo != null && AppSettings.IsAdminSite)
                {
                    return false;
                }

                if (AgentInfo != null && AdminInfo == null && AppSettings.IsAdminSite == IsDirectSignIn)
                {
                    return false;
                }

                return true;
            }
        }
    }
}