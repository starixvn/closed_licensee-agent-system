namespace LicenseeAgent.Site.Areas.Admin
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// AdminAreaRegistration class.
    /// </summary>
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_Maintenance",
                "Admin/Maintenance/{action}/{id}",
                new
                {
                    controller = "Maintenance",
                    action = "Password",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "LicenseeAgent.Web.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/admin/maintenance").Include(
                "~/Areas/Admin/Scripts/Maintenance/admin-maintenance-password.js").Include(
                "~/Areas/Admin/Scripts/Maintenance/admin-maintenance-syncmonitoring.js"));
        }
    }
}