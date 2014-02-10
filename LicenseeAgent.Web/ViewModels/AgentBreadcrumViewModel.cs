// -----------------------------------------------------------------------
// <copyright file="AgentBreadcrumViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentBreadcrumViewModel
    {
        public AgentBreadcrumViewModel()
        {
        }

        public AgentBreadcrumViewModel(int agentId, string agentName, short agentLevel)
        {
            AgentId = agentId;
            AgentName = agentName;
            AgentLevel = agentLevel;

            var list = new List<BreadcrumbItem>();
            list.Add(new BreadcrumbItem
                {
                    AgentId = agentId,
                    AgentLevel = agentLevel,
                    AgentName = agentName
                });

            Breadcrumb = new Breadcrumb(list);
        }

        public int AgentId { get; set; }

        public short AgentLevel { get; set; }

        public string AgentName { get; set; }

        public Breadcrumb Breadcrumb { get; set; }
    }
}