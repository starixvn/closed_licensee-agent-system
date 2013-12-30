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
        public CurrentSession(AdminInfo adminInfo, int checkinOnUserId, string language = "en-US")
        {
            AdminInfo = adminInfo;
            AgentInfo = null;
            Language = language;
            OnUserId = checkinOnUserId;
            IsDirectSignIn = true;
        }

        public CurrentSession(LicenseeInfo licenseeInfo, int checkinOnUserId, string language = "en-US")
        {
            LicenseeInfo = licenseeInfo;
            AgentInfo = null;
            Language = language;
            OnUserId = checkinOnUserId;
            IsDirectSignIn = true;
        }

        public CurrentSession(AgentInfo agentInfo, int checkinOnUserId, int checkinOnLicenseeId = 0, string language = "en-US")
        {
            AdminInfo = null;
            LicenseeInfo = null;
            AgentInfo = agentInfo;
            Language = language;
            OnUserId = checkinOnUserId;
            OnLicenseeId = checkinOnLicenseeId;
            IsDirectSignIn = true;
        }

        public AdminInfo AdminInfo { get; private set; }

        public LicenseeInfo LicenseeInfo { get; private set; }

        public AgentInfo AgentInfo { get; set; }

        public bool IsDirectSignIn { get; set; }

        public string Language { get; set; }

        public int OnUserId { get; private set; }

        public int OnLicenseeId { get; private set; }

        public bool IsValid
        {
            get
            {
                if (AdminInfo == null && LicenseeInfo == null && AgentInfo == null)
                {
                    return false;
                }

                if (!AppSettings.IsAdminSite && AdminInfo != null)
                {
                    return false;
                }

                if (AppSettings.IsAdminSite && LicenseeInfo != null)
                {
                    return false;
                }

                return true;
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
                else if (LicenseeInfo != null)
                {
                    return LicenseeInfo.UserId;
                }
                else
                {
                    return AgentInfo.AgentId;
                }
            }
        }
    }
}