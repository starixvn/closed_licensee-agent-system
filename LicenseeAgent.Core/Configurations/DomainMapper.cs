namespace LicenseeAgent.Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.Caching;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// DomainMapper class.
    /// </summary>
    public static class DomainMapper
    {
        private const string LicenseDomainsCacheKey = "LAS_LICENSEE_DOMAINS";
        private const int LicenseeDomainsCacheTimeout = 30 * 60; // 60 minutes
        private static ICoreService _coreService = new CoreService();
        private static ICache _cacheService = Fanex.Caching.CacheService.CreateInstance();

        /// <summary>
        /// Gets licensee id by URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Returns licensee id.
        /// </returns>
        public static int? GetLicenseeId(HttpRequestBase request)
        {
            var licenseeDomains = GetLicenseeDomains();
            licenseeDomains = licenseeDomains.Where(x => x.DomainName.IndexOf(request.Url.Host, StringComparison.OrdinalIgnoreCase) != -1
                && string.Compare(x.Environment, AppSettings.Environment, StringComparison.OrdinalIgnoreCase) == 0);
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

        /// <summary>
        /// Gets the licensee identifier.
        /// </summary>
        /// <param name="licenseeName">Name of the licensee.</param>
        /// <returns>Returns licensee id.</returns>
        public static int? GetLicenseeId(string licenseeName)
        {
            var licenseeDomains = GetLicenseeDomains();
            licenseeDomains = licenseeDomains.Where(x => x.LicenseeName.Equals(licenseeName, StringComparison.OrdinalIgnoreCase)
                && string.Compare(x.Environment, AppSettings.Environment, StringComparison.OrdinalIgnoreCase) == 0);

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
                if (licenseeDomains == null || !licenseeDomains.Any())
                {
                    throw new InvalidOperationException("Licensee domains configuration is fail.");
                }

                _cacheService.Set(LicenseDomainsCacheKey, licenseeDomains, LicenseeDomainsCacheTimeout);
            }

            return licenseeDomains;
        }
    }
}