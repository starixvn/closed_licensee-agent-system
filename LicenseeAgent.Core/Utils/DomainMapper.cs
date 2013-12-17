// --------------------------------------------------------------------------
// <copyright file="DomainMapper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.10.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Linq;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DomainMapper
    {
        private static IEnumerable<LicenseeDomain> _domains = null;

        static DomainMapper()
        {
            var httpContext = HttpContext.Current;
            var domainsFilePath = Path.Combine(httpContext.Request.PhysicalApplicationPath, "App_Data", "Domains.xml");
            var document = XDocument.Load(domainsFilePath);
            var documentRoot = document.Root;
            _domains = documentRoot.Elements("Domain").Select(
                x => new LicenseeDomain
                {
                    Host = x.Attribute("host").Value,
                    Licensee = x.Attribute("licensee").Value
                });
        }

        /// <summary>
        /// Gets the domain by URL.
        /// </summary>
        /// <param name="url">The request URL.</param>
        /// <returns>Returns domain name.</returns>
        public static string GetDomain(Uri url)
        {
            var host = url.Host.Trim();
            var licensee = _domains.Where(x => x.Host == host);
            if (licensee.Count() == 1)
            {
                return licensee.First().Licensee;
            }

            return string.Empty;
        }
    }
}