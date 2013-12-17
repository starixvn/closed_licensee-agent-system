// --------------------------------------------------------------------------
// <copyright file="CurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.6">Created</change>
// </history>

namespace LicenseeAgent.Web
{
    using System;
    using System.Collections.Generic;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    ///  It contains all properties of an authenticated user.
    ///  The implementation is applied when user config is state server mode.
    /// </summary>
    [Serializable]
    public class CurrentSession : ICurrentSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession"/> class.
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <param name="checkinOnUserId">The check in configuration user unique identifier.</param>
        /// <param name="language">The language.</param>
        public CurrentSession(AgentInfo agentInfo, int checkinOnUserId, string language = "en-US")
        {
            AgentInfo = agentInfo;
            AdminInfo = null;
            IsInternalSignIn = true;
            Language = language;
            OnUserId = checkinOnUserId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession"/> class.
        /// </summary>
        /// <param name="adminInfo">The admin information.</param>
        /// <param name="checkinOnUserId">The check in configuration user unique identifier.</param>
        /// <param name="language">The language.</param>
        public CurrentSession(AdminInfo adminInfo, int checkinOnUserId, string language = "en-US")
        {
            AdminInfo = adminInfo;
            AgentInfo = null;
            IsInternalSignIn = true;
            Language = language;
            OnUserId = checkinOnUserId;
        }

        public AgentInfo AgentInfo { get; set; }

        public AdminInfo AdminInfo { get; private set; }

        public InternalLoginInfo InternalLoginInfo { get; set; }

        public string Language { get; set; }

        public int OnUserId { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is internal sign in].
        /// </summary>
        /// <value>
        ///   <c>true</c> if sign-in directly; otherwise, <c>false (LRF, login as agent)</c>.
        /// </value>
        public bool IsInternalSignIn { get; set; }

        public int ServerId
        {
            get
            {
                return AgentInfo != null ? AgentInfo.ServerId : AdminInfo.ServerId;
            }
        }

        public string UserName
        {
            get
            {
                return AgentInfo != null && !AgentInfo.IsLevel0 ? AgentInfo.UserName : AdminInfo.UserName;
            }
        }

        public bool IsValid
        {
            get
            {
                if (AdminInfo == null && AgentInfo == null)
                {
                    return false;
                }

                // admin direct login
                if (AdminInfo != null && IsInternalSignIn && !AppSettings.IsInternal)
                {
                    return false;
                }

                // agent direct login
                if (AdminInfo == null && AgentInfo != null && IsInternalSignIn && AppSettings.IsInternal)
                {
                    return false;
                }

                // admin login as agent
                if (AppSettings.IsInternal && !IsInternalSignIn && InternalLoginInfo == null)
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
                return AdminInfo != null || InternalLoginInfo != null;
            }
        }

        public int LoginUserId
        {
            get
            {
                if (AdminInfo != null)
                {
                    return AdminInfo.UserId;
                }
                else
                {
                    if (IsInternalSignIn)
                    {
                        return AgentInfo.AgentId;
                    }
                    else
                    {
                        return InternalLoginInfo.AdminId;
                    }
                }
            }
        }
    }
}