namespace LicenseeAgent.Site.Areas.MemberReArrangement
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Member Arrangement Area Registration.
    /// </summary>
    public class MemberArrangementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MemberArrangement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MemberArrangement_default",
                "MemberArrangement/{action}/{id}",
                new { controller = "MemberArrangement", action = "Index", id = UrlParameter.Optional },
                new[] { "LicenseeAgent.MemberArrangement.Controllers" });
            RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register Bundles for Commission Group.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/member-arrangement").Include(
                "~/Areas/MemberArrangement/Scripts/member-arrangement.js"));
        }
    }
}