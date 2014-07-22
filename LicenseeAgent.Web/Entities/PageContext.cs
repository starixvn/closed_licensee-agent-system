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
        public const int ReportMaxMonth = 36;

        public PageContext(ICurrentSession currentSession, int agentId, int agentLevel)
            : this(currentSession, agentId, agentLevel, string.Empty)
        {
        }

        public PageContext(ICurrentSession currentSession, int agentId, int agentLevel, string agentName)
            : this(currentSession)
        {
            AgentId = agentId;
            AgentLevel = agentLevel;
            AgentName = agentName;
            IsDirectDownLine = currentSession.IsDirectDownLine(agentLevel);
            IsMember = false;
        }

        public PageContext(ICurrentSession currentSession, int custId)
            : this(currentSession)
        {
            CustId = custId;
            IsDirectDownLine = currentSession.AgentInfo.IsLastLevel;
            IsMember = true;
        }

        public PageContext(ICurrentSession currentSession)
        {
            IsAdmin = currentSession.IsAdmin;
            LegendType = (int)currentSession.AgentInfo.LegendType;
            AgentId = currentSession.AgentInfo.AgentId;
            AgentName = currentSession.AgentInfo.UserName;
            AgentLevel = currentSession.AgentInfo.AgentLevel;
            NumberOfLevel = (int)currentSession.AgentInfo.NumberOfLevel;
            IsDirectDownLine = true;
            MaxDate = DateTime.Now.Date;
            MinDate = MaxDate.AddMonths(-ReportMaxMonth);
        }

        public bool IsAdmin { get; private set; }

        public int LegendType { get; private set; }

        public int AgentId { get; private set; }

        public string AgentName { get; private set; }

        public int AgentLevel { get; private set; }

        public int NumberOfLevel { get; private set; }

        public bool IsDirectDownLine { get; private set; }

        public int CustId { get; private set; }

        public bool IsLastLevel
        {
            get
            {
                return ModelHelper.IsLastAgent(AgentLevel, NumberOfLevel);
            }
        }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        public bool IsMember { get; set; }

        public bool IsOfferedVirtualSports { get; set; }
    }
}