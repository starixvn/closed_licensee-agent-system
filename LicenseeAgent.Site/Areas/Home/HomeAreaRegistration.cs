namespace LicenseeAgent.Site.Areas.Home
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// AccountAreaRegistration class.
    /// </summary>
    public class HomeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Home";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Home_Default",
                "Home/{controller}/{action}",
                new[] { "LicenseeAgent.Home.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/home/top-menu").Include(
                "~/Areas/Home/Scripts/TopMenu/top-menu.js"));

            bundles.Add(new ScriptBundle("~/js/home/account-information").Include(
                "~/Areas/Home/Scripts/AccountInformation/account-information.js"));
            bundles.Add(new LessBundle("~/css/home/account-information").Include(
                "~/Areas/Home/Content/AccountInformation/account-information.less"));
        }
    }
}