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
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Caching;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DomainMapper
    {
        private const string LicenseDomainsCacheKey = "LAS_LICENSEE_DOMAINS";
        private const int LicenseeDomainsCacheTimeout = 30 * 60; // 60 minutes
        private static ICoreService _coreService = DependencyResolver.Current.GetService<ICoreService>();
        private static ICache _cacheService = CacheService.CreateInstance();

        /// <summary>
        /// Gets licensee id by URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Returns domain name.
        /// </returns>
        public static int? GetLicenseeId(HttpRequestBase request)
        {
            var licenseeDomains = GetLicenseeDomains();
            licenseeDomains = licenseeDomains.Where(x => x.DomainName.Equals(request.Url.Authority, StringComparison.OrdinalIgnoreCase));
            if (licenseeDomains.Count() == 1)
            {
                return licenseeDomains.First().LicenseeId;
            }

            if (AppSettings.LocalTest)
            {
                return AppSettings.LocalLicenseeId;
            }

            return null;
        }

        public static string GetDomain(int licenseeId)
        {
            var licenseeDomains = GetLicenseeDomains();
            licenseeDomains = licenseeDomains.Where(x => x.LicenseeId.Equals(licenseeId));
            if (licenseeDomains.Count() == 1)
            {
                return licenseeDomains.First().DomainName;
            }

            return null;
        }

        /// <summary>
        /// Gets the licensee domains.
        /// </summary>
        /// <returns>Returns list of licensee domains.</returns>
        /// <exception cref="System.Exception">Licensee domains configuration is fail.</exception>
        private static IEnumerable<LicenseeDomain> GetLicenseeDomains()
        {
            var licenseeDomains = _cacheService.Get<IEnumerable<LicenseeDomain>>(LicenseDomainsCacheKey);
            if (licenseeDomains == null)
            {
                licenseeDomains = _coreService.GetLicenseeDomains();
                if (licenseeDomains == null || licenseeDomains.Count() == 0)
                {
                    throw new InvalidOperationException("Licensee domains configuration is fail");
                }

                _cacheService.Set<IEnumerable<LicenseeDomain>>(LicenseDomainsCacheKey, licenseeDomains, LicenseeDomainsCacheTimeout);
            }

            return licenseeDomains;
        }
    }
}