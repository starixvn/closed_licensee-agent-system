// --------------------------------------------------------------------------
// <copyright file="CoreService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Services
{
    using System.Collections.Generic;
    using Fanex.Caching;
    using LicenseeAgent.Core.Criterias;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CoreService : QueryService, ICoreService
    {
        private const string BalanceUpDayCacheKey = "BalanceUpDayCacheKey";

        private ICache _cache = CacheService.CreateInstance(CacheType.MemoryCache);

        /// <summary>
        /// Try to get balance up information from cache and database.
        /// </summary>
        /// <returns>Balance Up Information.</returns>
        public SelBalanceUpDay GetBalanceUpDay()
        {
            if (_cache.Contains(BalanceUpDayCacheKey))
            {
                return _cache.Get<SelBalanceUpDay>(BalanceUpDayCacheKey);
            }

            var criteria = new SelBalanceUpDayCriteria();
            var balanceUpDay = GetEntity<SelBalanceUpDay>(criteria);
            _cache.Set(BalanceUpDayCacheKey, balanceUpDay);

            return balanceUpDay;
        }

        public IEnumerable<LicenseeDomain> GetLicenseeDomains()
        {
            var criteria = new GetLicenseeDomainCriteria();
            return GetEntites<LicenseeDomain>(criteria);
        }
    }
}