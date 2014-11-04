namespace LicenseeAgent.Site.Areas.MemberReArrangement
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Member Re-Arrangement Area Registration.
    /// </summary>
    public class MemberReArrangementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MemberReArrangement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                     "MemberReArrangement_default",
                     "MemberReArrangement/{controller}/{action}/{id}",
                     new { controller = "MemberReArrangement", action = "Index", id = UrlParameter.Optional },
                     new[] { "LicenseeAgent.MemberReArrangement.Controllers" });
            RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register Bundles for Commission Group.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/memberarrangement").Include(
                "~/Areas/MemberReArrangement/Scripts/MemberRearrangement/MemberArrangement.js"));
        }
    }
}
