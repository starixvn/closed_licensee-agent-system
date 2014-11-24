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

            context.MapRoute(
                "Report_AgentWinLoss",
                "Report/AgentWinLoss/{action}/{id}",
                new
                {
                    controller = "AgentWinLoss",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "LicenseeAgent.Report.Controllers" });

            context.MapRoute(
                "Report_AgentOutstanding",
                "Report/AgentOutstanding/{action}/{id}",
                new
                {
                    controller = "AgentOutstanding",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "LicenseeAgent.Report.Controllers" });

            context.MapRoute(
                "Report_Result",
                "Report/Results/{action}/{id}",
                new
                {
                    controller = "Results",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "LicenseeAgent.Report.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/report/statement").Include(
                "~/Areas/Report/Scripts/Statement/report-statement.js").Include(
                "~/Areas/Report/Scripts/Statement/report-statement-detail.js").Include(
                "~/Areas/Report/Scripts/Statement/report-statement-transferDetail.js"));

            bundles.Add(new ScriptBundle("~/js/report/agent-win-loss").Include(
                "~/Areas/Report/Scripts/AgentWinLoss/report-agent-winloss.js"));

            bundles.Add(new ScriptBundle("~/js/report/agent-outstanding").Include(
                "~/Areas/Report/Scripts/AgentOutstanding/report-agent-outstanding.js").Include(
                "~/Views/BetList/Resources/betlist-core.js"));
            bundles.Add(new LessBundle("~/css/report/agent-outstanding").Include(
                "~/Areas/Report/Content/agent-outstanding.less"));

            bundles.Add(new ScriptBundle("~/js/report/results").Include(
                "~/Areas/Report/Scripts/Results/result-report.js"));

            bundles.Add(new LessBundle("~/css/report/results").Include(
                "~/Content/Fanex.Result/fanex.result.less"));
        }
    }
}