namespace LicenseeAgent.Tests.Fakes
{
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;

    /// <summary>
    /// FakeControllerContext class.
    /// </summary>
    public class FakeControllerContext : ControllerContext
    {
        public FakeControllerContext(ControllerBase controller)
            : this(controller, null, null, null, null, null, null)
        {
        }

        public FakeControllerContext(ControllerBase controller, HttpCookieCollection cookies)
            : this(controller, null, null, null, null, cookies, null)
        {
        }

        public FakeControllerContext(ControllerBase controller, SessionStateItemCollection sessionItems)
            : this(controller, null, null, null, null, null, sessionItems)
        {
        }

        public FakeControllerContext(ControllerBase controller, NameValueCollection formParams)
            : this(controller, null, null, formParams, null, null, null)
        {
        }

        public FakeControllerContext(ControllerBase controller, NameValueCollection formParams, NameValueCollection queryStringParams)
            : this(controller, null, null, formParams, queryStringParams, null, null)
        {
        }

        public FakeControllerContext(ControllerBase controller, string userName)
            : this(controller, userName, null, null, null, null, null)
        {
        }

        public FakeControllerContext(ControllerBase controller, string userName, string[] roles)
            : this(controller, userName, roles, null, null, null, null)
        {
        }

        public FakeControllerContext(
            ControllerBase controller,
            string userName,
            string[] roles,
            NameValueCollection formParams,
            NameValueCollection queryStringParams,
            HttpCookieCollection cookies,
            SessionStateItemCollection sessionItems)
            : this(
            new FakeHttpContext(
                new FakePrincipal(new FakeIdentity(userName), roles),
                formParams,
                queryStringParams,
                cookies,
                sessionItems),
            controller)
        {
        }

        public FakeControllerContext(ControllerBase controller, FakePrincipal principal)
            : this(new FakeHttpContext(principal, null, null, null, null), controller)
        {
        }

        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Verified")]
        public FakeControllerContext(FakeHttpContext fakeHttpContext, ControllerBase controller)
            : base(fakeHttpContext, new RouteData(), controller)
        {
            var currentHttpContext = new HttpContext(
                new HttpRequest(string.Empty, HttpContext.Request.Url.AbsoluteUri, string.Empty),
                new HttpResponse(new StringWriter()));

            currentHttpContext.User = HttpContext.User;
            var sessionContainer = new HttpSessionStateContainer(
                "id",
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);
            currentHttpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                CallingConventions.Standard,
                new[] { typeof(HttpSessionStateContainer) },
                null).Invoke(new object[] { sessionContainer });

            System.Web.HttpContext.Current = currentHttpContext;
        }
    }
}