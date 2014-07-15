namespace LicenseeAgent.Web.Entities
{
    using System;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Web;

    /// <summary>
    /// PageContext class.
    /// </summary>
    [Serializable]
    public class PageContext
    {
        public PageContext(ICurrentSession currentSession, int agentId, int agentLevel)
            : this(currentSession)
        {
            AgentId = agentId;
            AgentLevel = agentLevel;
            IsDirectDownline = currentSession.IsDirectDownline(agentLevel);
        }

        public PageContext(ICurrentSession currentSession)
        {
            IsAdmin = currentSession.IsAdmin;
            LegendType = (int)currentSession.AgentInfo.LegendType;
            AgentId = currentSession.AgentInfo.AgentId;
            AgentLevel = currentSession.AgentInfo.AgentLevel;
            NumberOfLevel = (int)currentSession.AgentInfo.NumberOfLevel;
            IsDirectDownline = true;
        }

        public bool IsAdmin { get; private set; }

        public int LegendType { get; private set; }

        public int AgentId { get; private set; }

        public int AgentLevel { get; private set; }

        public int NumberOfLevel { get; private set; }

        public bool IsDirectDownline { get; private set; }

        public bool IsLastLevel
        {
            get
            {
                return ModelHelper.IsLastAgent(AgentLevel, NumberOfLevel);
            }
        }

        public bool IsMember { get; set; }

        public bool IsOfferedVirtualSports { get; set; }
    }
}