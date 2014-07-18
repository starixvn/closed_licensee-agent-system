namespace LicenseeAgent.API.LRF
{
    using System.Configuration;

    /// <summary>
    /// Configuration class.
    /// </summary>
    public static class Configuration
    {
        static Configuration()
        {
            LegendEnvironment = ConfigurationManager.AppSettings["LegendEnvironment"];
            LegendAdminSite = ConfigurationManager.AppSettings["LegendAdminSite"];
        }

        public static string LegendEnvironment { get; private set; }

        public static string LegendAdminSite { get; private set; }
    }
}