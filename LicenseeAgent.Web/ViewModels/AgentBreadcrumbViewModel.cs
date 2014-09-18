namespace LicenseeAgent.Web.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Agent breadcrumb view modal.
    /// </summary>
    public class AgentBreadcrumbViewModel
    {
        public AgentBreadcrumbViewModel()
        {
        }

        public AgentBreadcrumbViewModel(int agentId, string agentName, short agentLevel, bool isMember = false)
        {
            AgentId = agentId;
            AgentName = agentName;
            AgentLevel = agentLevel;
            IsMember = isMember;

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

        public bool IsMember { get; set; }

        public Breadcrumb Breadcrumb { get; set; }
    }
}