namespace LicenseeAgent.Site.Areas.CommissionGroup
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Commission Group Area Registration.
    /// </summary>
    public class CommissionSettingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CommissionSetting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CommissionSetting_Default",
                "CommissionSetting/{controller}/{action}/{id}",
                new { controller = "GroupList", action = "Index", id = UrlParameter.Optional },
                new[] { "LicenseeAgent.CommissionSetting.Controllers" });
            RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register Bundles for Commission Group.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/commission-setting").Include(
                "~/Areas/CommissionSetting/Scripts/commission-setting.js",
                "~/Areas/CommissionSetting/Scripts/GroupList/group-list.js",
                "~/Areas/CommissionSetting/Scripts/GroupList/group-crud.js",
                "~/Areas/CommissionSetting/Scripts/MemberList/member-list.js",
                "~/Areas/CommissionSetting/Scripts/MemberList/member-crud.js"));

            bundles.Add(new LessBundle("~/css/commission-setting").Include(
                "~/Areas/CommissionSetting/Content/commission-setting.less"));
        }
    }
}