namespace LicenseeAgent.Site.Areas.TotalBets
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// TotalBetsAreaRegistration class.
    /// </summary>
    public class TotalBetsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TotalBets";
            }
        }

        /// <summary>
        ///  Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TotalBets_Default",
                "TotalBets/{controller}/{action}/{id}",
                new
                {
                    controller = "Soccer",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                new[] { "LicenseeAgent.TotalBets.Controllers" });

            RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register Bundles for Total bets.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/totalbets").Include(
                "~/Areas/TotalBets/Content/totalbets.less"));

            bundles.Add(new ScriptBundle("~/js/totalbets").Include(
                "~/Areas/TotalBets/Scripts/totalBets.js",
                "~/Areas/TotalBets/Scripts/Soccer/soccer.js",
                "~/Areas/TotalBets/Scripts/Soccer/soccer-detail.js",
                "~/Areas/TotalBets/Scripts/Basketball/basketball.js",
                "~/Areas/TotalBets/Scripts/OtherSports/othersports.js",
                "~/Areas/TotalBets/Scripts/Outright/outright.js",
                "~/Areas/TotalBets/Scripts/Parlay/parlay.js",
                "~/Views/BetList/Scripts/core.js"));
        }
    }
}