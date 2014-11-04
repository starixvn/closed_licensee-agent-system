namespace LicenseeAgent.Site.Areas.CommissionGroup
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Commission Group Area Registration.
    /// </summary>
    public class CommissionGroupAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CommissionGroup";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                    "CommissionGroup_default",
                    "CommissionGroup/{controller}/{action}/{id}",
                    new { controller = "Group", action = "Index", id = UrlParameter.Optional },
                    new[] { "LicenseeAgent.CommissionGroup.Controllers" });
            RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register Bundles for Commission Group.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/CommissionGroup").Include(
                "~/Areas/CommissionGroup/Scripts/Group/CommissionGroup.js"));

            bundles.Add(new ScriptBundle("~/js/CommissionGroupUtility").Include(
                "~/Areas/CommissionGroup/Scripts/Group/CommissionGroupUtility.js"));

            bundles.Add(new ScriptBundle("~/js/membercommission").Include(
                "~/Areas/CommissionGroup/Scripts/Member/member-commission.js"));

            bundles.Add(new LessBundle("~/css/membercommission").Include(
                "~/Areas/CommissionGroup/Content/member-commission.less"));
        }
    }
}