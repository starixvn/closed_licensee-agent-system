namespace LicenseeAgent.Site.Areas.Account
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// AccountAreaRegistration class.
    /// </summary>
    public class AuthenticationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Authentication";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Authentication_Internal",
                "Internal/{action}/{id}",
                new
                {
                    controller = "Internal",
                    id = UrlParameter.Optional
                },
                new[] { "LicenseeAgent.Authentication.Controllers" });

            context.MapRoute(
                "Authentication_Default",
                "Authentication/{controller}/{action}/{id}",
                new
                {
                    controller = "Agent",
                    action = "SignIn",
                    id = UrlParameter.Optional
                },
                new[] { "LicenseeAgent.Authentication.Controllers" });
        }
    }
}