namespace LicenseeAgent.Site.Areas.Customer
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// CustomerAreaRegistration class.
    /// </summary>
    public class CustomerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Customer";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Customer_AccountInformation",
                "Customer/AccountInformation/{action}/{id}",
                new
                {
                    controller = "AccountInformation",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                new[] { "LicenseeAgent.AccountInformation.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/customer/accountinformation").Include(
                "~/Areas/Customer/Scripts/AccountInformation/account-information.js"));
            bundles.Add(new LessBundle("~/css/customer/accountinformation").Include(
                "~/Areas/Customer/Content/AccountInformation/account-information.less"));
        }
    }
}