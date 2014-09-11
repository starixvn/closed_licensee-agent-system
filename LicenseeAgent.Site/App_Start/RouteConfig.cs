namespace LicenseeAgent.Site
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// MVC Route Configuration.
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("content/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("scripts/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute("AccessDenied", "AccessDenied", new { controller = "Error", action = "AccessDenied" });
            routes.MapRoute("CurrencyPermissionDenied", "CurrencyPermissionDenied", new { controller = "Error", action = "CurrencyPermissionDenied" });
            routes.MapRoute("Error", "Error/{statusCode}", new { controller = "Error", action = "Index", statusCode = UrlParameter.Optional });
            routes.MapRoute("Unauthorized", "Unauthorized", new { controller = "Error", action = "Unauthorized" });
            routes.MapRoute("UnderMaintenance", "UnderMaintenance", new { controller = "Error", action = "UnderMaintenance" });
            routes.MapRoute("WhoAmI", "WhoAmI", new { controller = "Error", action = "WhoAmI" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                });
        }
    }
}