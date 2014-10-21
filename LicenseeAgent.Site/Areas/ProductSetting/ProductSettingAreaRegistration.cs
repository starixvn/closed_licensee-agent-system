namespace LicenseeAgent.Site.Areas.ProductSetting
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// CustomerAreaRegistration class.
    /// </summary>
    public class ProductSettingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ProductSetting";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ProductSetting_NewAgent",
                "ProductSetting/NewAgent",
                new
                {
                    controller = "SportsBook",
                    action = "InsertCustomer"
                },
                new[] { "LicenseeAgent.ProductSetting.Controllers" });

            context.MapRoute(
                "ProductSetting_Default",
                "ProductSetting/{controller}/{action}/{id}",
                new
                {
                    controller = "SportsBook",
                    action = "InsertCustomer",
                    id = UrlParameter.Optional
                },
                new[] { "LicenseeAgent.ProductSetting.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/productsetting").IncludeDirectory(
                "~/Areas/ProductSetting/Scripts/Shared", "*.js", true));

            bundles.Add(new ScriptBundle("~/js/productsetting/sportsbook").IncludeDirectory(
                "~/Areas/ProductSetting/Scripts/SportsBook", "*.js", true).IncludeDirectory(
                "~/Areas/ProductSetting/Scripts/NumberGame", "*.js", true));

            bundles.Add(new ScriptBundle("~/js/productsetting/thirdproduct").IncludeDirectory(
               "~/Areas/ProductSetting/Scripts/ThirdProduct", "*.js", true));

            bundles.Add(new ScriptBundle("~/js/productsetting/virtualsports").IncludeDirectory(
               "~/Areas/ProductSetting/Scripts/VirtualSports", "*.js", true));

            bundles.Add(new ScriptBundle("~/js/productsetting/keno").IncludeDirectory(
               "~/Areas/ProductSetting/Scripts/Keno", "*.js", true));

            bundles.Add(new LessBundle("~/css/productsetting").Include(
                "~/Areas/ProductSetting/Content/fanex.customerSetting.less"));

            bundles.Add(new LessBundle("~/css/productsetting/editproduct").Include(
                "~/Areas/ProductSetting/Content/edit-product.less"));
        }
    }
}