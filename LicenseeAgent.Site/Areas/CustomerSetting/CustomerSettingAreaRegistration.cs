namespace LicenseeAgent.Site.Areas.Customer
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// CustomerAreaRegistration class.
    /// </summary>
    public class CustomerSettingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CustomerSetting";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CustomerSetting_NewAgent",
                "CustomerSetting/NewAgent",
                new
                {
                    controller = "SportsBook",
                    action = "InsertCustomer"
                },
                new[] { "LicenseeAgent.CustomerSetting.Controllers.Products" });

            context.MapRoute(
                "CustomerSetting_Product",
                "CustomerSetting/{controller}/{action}/{id}",
                new
                {
                    action = "InsertCustomer",
                    id = UrlParameter.Optional
                },
                new[] { "LicenseeAgent.CustomerSetting.Controllers.Products" });

            RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/customersetting").IncludeDirectory(
                "~/Areas/CustomerSetting/Scripts/Products/Shared", "*.js", true));

            bundles.Add(new ScriptBundle("~/js/customersetting/sportsbook").IncludeDirectory(
                "~/Areas/CustomerSetting/Scripts/Products/SportsBook", "*.js", true).IncludeDirectory(
                "~/Areas/CustomerSetting/Scripts/Products/NumberGame", "*.js", true));

            bundles.Add(new ScriptBundle("~/js/customersetting/virtualsports").IncludeDirectory(
               "~/Areas/CustomerSetting/Scripts/Products/VirtualSports", "*.js", true));

            bundles.Add(new LessBundle("~/css/customersetting").Include(
                "~/Areas/CustomerSetting/Content/Products/fanex.customerSetting.less"));

            bundles.Add(new LessBundle("~/css/customersetting/editproduct").Include(
                "~/Areas/CustomerSetting/Content/Products/edit-product.less"));
        }
    }
}