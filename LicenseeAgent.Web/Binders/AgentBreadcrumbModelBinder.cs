namespace LicenseeAgent.Web.Binders
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Services;
    using LicenseeAgent.Web.ViewModels;

    /// <summary>
    /// Breadcrumb binder class.
    /// </summary>
    public class AgentBreadcrumbModelBinder : IModelBinder
    {
        private readonly IDownLinePermissionService _downLinePermissionService;
        private readonly ICoreService _coreService;

        public AgentBreadcrumbModelBinder()
            : this(null, null)
        {
        }

        public AgentBreadcrumbModelBinder(
            IDownLinePermissionService downLinePermissionService,
            ICoreService coreService)
        {
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _coreService = coreService ?? new CoreService();
        }

        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The bound value.
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var currentSession = SessionManager.CurrentSession;
            var request = controllerContext.HttpContext.Request;
            var agentId = 0;
            AgentInfo agentInfo = null;
            if (int.TryParse(request.QueryString.Get("AgentId"), out agentId))
            {
                agentInfo = currentSession.AgentInfo;
            }
            else if (_downLinePermissionService.IsDirectDownLineAgent(agentId))
            {
                agentInfo = _coreService.GetAgentInfo(agentId);
            }

            if (agentInfo == null)
            {
                bindingContext.ModelState.AddModelError(string.Empty, "Data not found");
                return null;
            }

            var breadcrumb = Breadcrumb.FromString(request.QueryString.Get("Breadcrumb"));
            breadcrumb.Join(new BreadcrumbItem
            {
                AgentId = agentInfo.AgentId,
                AgentName = agentInfo.UserName,
                AgentLevel = agentInfo.AgentLevel,
            });

            return new AgentBreadcrumbViewModel
            {
                AgentId = agentInfo.AgentId,
                AgentName = agentInfo.UserName,
                AgentLevel = agentInfo.AgentLevel,
                Breadcrumb = breadcrumb
            };
        }
    }
}