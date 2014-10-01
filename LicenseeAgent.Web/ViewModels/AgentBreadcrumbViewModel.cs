namespace LicenseeAgent.Web.ViewModels
{
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Agent breadcrumb view modal.
    /// </summary>
    public class AgentBreadcrumbViewModel
    {
        public int AgentId { get; set; }

        public short AgentLevel { get; set; }

        public string AgentName { get; set; }

        public Breadcrumb Breadcrumb { get; set; }
    }
}