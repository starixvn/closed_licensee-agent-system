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
        /// <param name="url">The request URL.</param>
        /// <returns>Returns domain name.</returns>
        public static int? GetLicenseeId(HttpRequestBase request)
        {
            if (request.IsLocal)
            {
                return AppSettings.LocalLicenseeId;
            }

            var licensee = _licenseeDomains.Where(x => x.DomainName.Equals(request.Url.Authority, StringComparison.OrdinalIgnoreCase));
            if (licensee.Count() == 1)
            {
                return licensee.First().LicenseeId;
            }

            return null;
        }
    }
}