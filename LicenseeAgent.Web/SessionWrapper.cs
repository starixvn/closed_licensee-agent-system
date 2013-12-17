// -----------------------------------------------------------------------
// <copyright file="SessionWrapper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SessionWrapper
    {
        private ICurrentSession _currentSession;

        public SessionWrapper(ICurrentSession currentSession)
        {
            _currentSession = currentSession;
        }

        public SessionWrapper()
            : this(SessionManager.CurrentSession)
        {
        }

        public bool IsViewSelectMenu
        {
            get
            {
                return (_currentSession.AgentInfo == null
                    || (_currentSession.AgentInfo != null && _currentSession.AgentInfo.IsLevel0))
                    ? true
                    : false;
            }
        }

        public bool IsViewChangePassword
        {
            get
            {
                return _currentSession.AgentInfo != null
                    && !_currentSession.AgentInfo.IsLevel0
                    ? true
                    : false;
            }
        }

        public bool IsDirectDownline(int agentLevel)
        {
            return _currentSession.AgentInfo.IsLevel0
                || agentLevel == _currentSession.AgentInfo.AgentLevel;
        }
    }
}