namespace LicenseeAgent.Site.Areas.Report
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// ReportAreaRegistration class.
    /// </summary>
    public class ReportAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Report";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Report_Statement",
                "Report/Statement/{action}/{id}",
                new
                {
                    controller = "Statement",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "LicenseeAgent.Report.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/report/statement").Include(
                "~/Areas/Report/Scripts/Statement/report-statement.js").Include(
                "~/Areas/Report/Scripts/Statement/report-statement-detail.js").Include(
                "~/Areas/Report/Scripts/Statement/report-statement-transferDetail.js"));
        }
    }
}