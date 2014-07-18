namespace LicenseeAgent.Tests.Fakes
{
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Web.SessionState;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class FakeSession
    {
        /// <summary>
        /// Initialize HTTP Context.
        /// </summary>
        public static void InitHttpContext()
        {
            var httpRequest = new HttpRequest(string.Empty, "http://localhost/", string.Empty);
            var httpResponce = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(httpRequest, httpResponce);
            var sessionContainer = new HttpSessionStateContainer(
                                                                "id",
                                                                new SessionStateItemCollection(),
                                                                new HttpStaticObjectsCollection(),
                                                                10,
                                                                true,
                                                                HttpCookieMode.AutoDetect,
                                                                SessionStateMode.InProc,
                                                                false);
            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                                                BindingFlags.NonPublic | BindingFlags.Instance,
                                                                null,
                                                                CallingConventions.Standard,
                                                                new[] { typeof(HttpSessionStateContainer) },
                                                                null).Invoke(new object[] { sessionContainer });
            HttpContext.Current = httpContext;
        }

        /// <summary>
        /// Set Session Admin Level.
        /// </summary>
        public static void SetAdminLevel()
        {
            SessionManager.CurrentSession = new CurrentSession(
                                                new AgentInfo
                                                {
                                                    AgentLevel = 1,
                                                    CurrencyId = 20,
                                                    CurrencyName = "UUS",
                                                    LicenseeId = 1,
                                                    LicenseeName = "yq355",
                                                    NumberOfLevel = 3
                                                },
                                                new InternalLoginInfo
                                                {
                                                    AgentName = "Agent",
                                                    CurrencyId = 20,
                                                    LicenseeId = 1,
                                                    LoginAccountType = AccountType.CSDIT,
                                                    LoginUserId = 10,
                                                    Language = "en-US"
                                                });
        }

        /// <summary>
        /// Set Session Agent Level.
        /// </summary>
        public static void SetAgentLevel()
        {
            SessionManager.CurrentSession = new CurrentSession(
                                                new AgentInfo
                                                {
                                                    AgentLevel = 2,
                                                    CurrencyId = 20,
                                                    CurrencyName = "UUS",
                                                    LicenseeId = 1,
                                                    LicenseeName = "yq355",
                                                    NumberOfLevel = 3
                                                },
                                                0,
                                                "en-US");
        }

        /// <summary>
        /// Set Session Agent Level.
        /// </summary>
        public static void SetLastAgentLevel()
        {
            SessionManager.CurrentSession = new CurrentSession(
                                                new AgentInfo
                                                {
                                                    AgentLevel = 3,
                                                    CurrencyId = 20,
                                                    CurrencyName = "UUS",
                                                    LicenseeId = 1,
                                                    LicenseeName = "yq355",
                                                    NumberOfLevel = 2
                                                },
                                                0,
                                                "en-US");
        }
    }
}