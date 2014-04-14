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

    public static class DomainMapper
    {
        private const string LicenseDomainsCacheKey = "LAS_LICENSEE_DOMAINS";
        private const int LicenseeDomainsCacheTimeout = 30 * 60; // 60 minutes
        private static readonly ICoreService CoreService = DependencyResolver.Current.GetService<ICoreService>();
        private static readonly ICache CacheService = Fanex.Caching.CacheService.CreateInstance();

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
            licenseeDomains = licenseeDomains.Where(x => x.DomainName.IndexOf(request.Url.Host, StringComparison.OrdinalIgnoreCase) != -1
                && string.Compare(x.Environment, AppSettings.Environment, true) == 0);
            if (licenseeDomains.Count() == 1)
            {
                return licenseeDomains.First().LicenseeId;
            }

            if (AppSettings.LocalTest || request.IsLocal)
            {
                return AppSettings.LocalLicenseeId;
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
            var licenseeDomains = CacheService.Get<IEnumerable<LicenseeDomain>>(LicenseDomainsCacheKey);
            if (licenseeDomains == null)
            {
                licenseeDomains = CoreService.GetLicenseeDomains();
                if (licenseeDomains == null || !licenseeDomains.Any())
                {
                    throw new InvalidOperationException("Licensee domains configuration is fail.");
                }

                CacheService.Set(LicenseDomainsCacheKey, licenseeDomains, LicenseeDomainsCacheTimeout);
            }

            return licenseeDomains;
        }
    }
}