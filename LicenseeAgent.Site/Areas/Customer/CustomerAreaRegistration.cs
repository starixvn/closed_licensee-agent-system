namespace LicenseeAgent.Site.Areas.Customer
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Customer Area Registration.
    /// </summary>
    public class CustomerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Customer";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                   "Customer",
                   "Customer/{controller}/{action}/{id}",
                   new
                   {
                       controller = "CustomerList",
                       action = "Index",
                       id = UrlParameter.Optional
                   },
                   new[] { "LicenseeAgent.Customer.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register Bundles for Customer.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/customer").Include(
                "~/Areas/Customer/Content/customer-list.less"));

            bundles.Add(new ScriptBundle("~/js/customer").Include(
                "~/Areas/Customer/Scripts/customer-list.js",
                "~/Areas/Customer/Scripts/pending-member-list.js",
                "~/Areas/Customer/Scripts/reset-password.js"));
        }
    }
}
