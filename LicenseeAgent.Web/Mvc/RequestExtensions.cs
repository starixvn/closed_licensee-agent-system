namespace LicenseeAgent.Web.Mvc
{
    using System;
    using System.Net;
    using System.Web;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Gets request information.
    /// </summary>
    public static class RequestExtensions
    {
        /// <summary>
        /// Gets the client information.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>Returns client information.</returns>
        public static ClientInfo GetClientInfo(this HttpRequestBase httpRequest)
        {
            var domainName = httpRequest.ServerVariables["SERVER_NAME"] != null ? httpRequest.ServerVariables["SERVER_NAME"].ToString() : string.Empty;
            var browserName = string.Empty;
            var platform = string.Empty;
            if (httpRequest.Browser.IsMobileDevice || httpRequest.UserAgent.ToLower().Contains("android"))
            {
                browserName = httpRequest.Browser.Browser + " " + httpRequest.Browser.MajorVersion;
                platform = httpRequest.UserAgent.ToLower().Contains("android") ? "andorid" : httpRequest.Browser.MobileDeviceModel;
            }
            else
            {
                browserName = httpRequest.Browser.Browser + " " + httpRequest.Browser.Version;
                platform = httpRequest.Browser.Platform;
            }

            return new ClientInfo
            {
                Ip = GetClientIp(httpRequest),
                DomainName = domainName,
                BrowserName = browserName,
                Platform = platform,
                CountryCode = string.Empty
            };
        }

        /// <summary>
        /// Gets the client IP.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>Returns client IP.</returns>
        public static string GetClientIp(this HttpRequestBase httpRequest)
        {
            string userHostAddress = httpRequest.UserHostAddress;
            var internalAddress = GetInternalAddress(userHostAddress);
            if (internalAddress == null)
            {
                var hostName = Dns.GetHostName();
                internalAddress = GetInternalAddress(hostName);
            }

            return internalAddress != null ? internalAddress.ToString() : string.Empty;
        }

        /// <summary>
        /// Gets the internal address.
        /// </summary>
        /// <param name="hostAddress">The host address.</param>
        /// <returns>Returns internal IP address.</returns>
        private static IPAddress GetInternalAddress(string hostAddress)
        {
            foreach (var itemIPA in Dns.GetHostAddresses(hostAddress))
            {
                if (itemIPA.AddressFamily.ToString().Equals("InterNetwork", StringComparison.InvariantCultureIgnoreCase))
                {
                    return itemIPA;
                }
            }

            return null;
        }
    }
}