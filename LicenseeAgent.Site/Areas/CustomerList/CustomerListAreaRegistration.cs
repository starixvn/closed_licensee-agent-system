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
                return "CustomerList";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                   "CustomerList_Default",
                   "CustomerList/{controller}/{action}/{id}",
                   new
                   {
                       controller = "CustomerList",
                       action = "Index",
                       id = UrlParameter.Optional
                   },
                   new[] { "LicenseeAgent.CustomerList.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register Bundles for Customer.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/customer-list").Include(
                "~/Areas/CustomerList/Content/customer-list.less"));

            bundles.Add(new ScriptBundle("~/js/customer-list/agent").Include(
                "~/Areas/CustomerList/Scripts/customer-list.js",
                "~/Areas/CustomerList/Scripts/Agent/agent.js"));

            bundles.Add(new ScriptBundle("~/js/customer-list/member").Include(
                "~/Areas/CustomerList/Scripts/customer-list.js",
                "~/Areas/CustomerList/Scripts/Member/member.js"));

            bundles.Add(new ScriptBundle("~/js/customer-list/pending-member").Include(
                "~/Areas/CustomerList/Scripts/PendingMember/pending-member.js"));

            bundles.Add(new ScriptBundle("~/js/customer-list/reset-password").Include(
                "~/Areas/CustomerList/Scripts/ResetPassword/reset-password.js"));
        }
    }
}