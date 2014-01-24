// -----------------------------------------------------------------------
// <copyright file="PageContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Entities
{
    using System;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class PageContext
    {
        public PageContext(int agentId, bool isLastLevel)
        {
            AgentId = agentId;
            IsLastLevel = isLastLevel;
        }

        public PageContext(int agentId, int agentLevel, int numberOfLevel)
        {
            AgentId = agentId;
            IsLastLevel = ModelHelper.IsLastAgent(agentLevel, numberOfLevel);
        }

        public int AgentId { get; set; }

        public bool IsLastLevel { get; set; }

        public string AgentName { get; set; }

        public bool? IsDirectDownline { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsMember { get; set; }
    }
}