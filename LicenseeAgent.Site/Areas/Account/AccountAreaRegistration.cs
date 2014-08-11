namespace LicenseeAgent.Site.Areas.Account
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// AccountAreaRegistration class.
    /// </summary>
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Account";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Account_HomePage",
                "Account/HomePage/{action}/{id}",
                new
                {
                    controller = "HomePage",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                new[] { "LicenseeAgent.Account.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/account/homepage").Include(
                "~/Areas/Account/Scripts/HomePage/account-homepage.js"));
            bundles.Add(new LessBundle("~/css/account/homepage").Include(
                "~/Areas/Account/Content/HomePage/account-homepage.less"));
        }
    }
}