namespace LicenseeAgent.Tests.Fakes
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Configuration;
    using LicenseeAgent.Tests.Configurations;

    /// <summary>
    /// FakeHttpRequest class.
    /// </summary>
    public class FakeHttpRequest : HttpRequestBase
    {
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;

        public FakeHttpRequest(NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies)
        {
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
        }

        public override NameValueCollection Form
        {
            get
            {
                return _formParams;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return _queryStringParams;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }

        public override Uri Url
        {
            get
            {
                return new Uri(Constants.LocalUrl);
            }
        }

        public override string UserHostAddress
        {
            get
            {
                return "127.0.0.1";
            }
        }

        public override NameValueCollection ServerVariables
        {
            get
            {
                var serverVariables = new NameValueCollection();
                serverVariables["SERVER_NAME"] = "localhost";
                return serverVariables;
            }
        }

        public override HttpBrowserCapabilitiesBase Browser
        {
            get
            {
                return new FakeHttpBrowserCapabilities();
            }
        }

        public override string UserAgent
        {
            get
            {
                return "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
            }
        }
    }
}