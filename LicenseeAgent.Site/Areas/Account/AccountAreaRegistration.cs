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
                "Account_Index",
                "Account",
                new
                {
                    controller = "HomePage",
                    action = "Index"
                },
                new[] { "LicenseeAgent.Account.Controllers" });

            context.MapRoute(
                "Account_HomePage",
                "Account/HomePage/{action}",
                new
                {
                    controller = "HomePage",
                    action = "Index"
                },
                new[] { "LicenseeAgent.Account.Controllers" });

            context.MapRoute(
                "Account_Default",
                "Account/{action}",
                new
                {
                    controller = "Account",
                    action = "Index"
                },
                new[] { "LicenseeAgent.Account.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/account/home-page").Include(
                "~/Areas/Account/Scripts/HomePage/home-page.js"));
            bundles.Add(new LessBundle("~/css/account/home-page").Include(
                "~/Areas/Account/Content/HomePage/home-page.less"));
        }
    }
}