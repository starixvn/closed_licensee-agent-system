// --------------------------------------------------------------------------
// <copyright file="DomainMapper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Logging;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DomainMapper
    {
        private static IEnumerable<LicenseeDomain> _licenseeDomains = null;

        static DomainMapper()
        {
            ICoreService coreService = DependencyResolver.Current.GetService<ICoreService>();
            _licenseeDomains = coreService.GetLicenseeDomains();
        }

        /// <summary>
        /// Gets licensee id by URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Returns domain name.
        /// </returns>
        public static int? GetLicenseeId(HttpRequestBase request)
        {
            if (AppSettings.LocalTest)
            {
                return AppSettings.LocalLicenseeId;
            }

            var licenseeDomains = _licenseeDomains.Where(x => x.DomainName.Equals(request.Url.Authority, StringComparison.OrdinalIgnoreCase));
            if (licenseeDomains.Count() == 1)
            {
                return licenseeDomains.First().LicenseeId;
            }

            return null;
        }

        public static string GetDomain(int licenseeId)
        {
            var licenseeDomains = _licenseeDomains.Where(x => x.LicenseeId.Equals(licenseeId));
            if (licenseeDomains.Count() == 1)
            {
                return licenseeDomains.First().DomainName;
            }

            return null;
        }
    }
}