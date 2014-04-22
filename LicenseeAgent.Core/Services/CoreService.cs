// --------------------------------------------------------------------------
// <copyright file="CoreService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Services
{
    using System.Collections.Generic;
    using Fanex.Caching;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Criterias;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// CoreService class.
    /// </summary>
    public class CoreService : QueryService, ICoreService
    {
        private const string BalanceUpDayCacheKey = "LAS_BalanceUpDayCacheKey";

        private readonly ICache _cache = CacheService.CreateInstance(CacheType.MemoryCache);

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

        /// <summary>
        /// Gets the licensee domains. If licenseeId=0 then get all licensee domains.
        /// </summary>
        /// <returns>Returns list of licensee domain.</returns>
        public IEnumerable<LicenseeDomain> GetLicenseeDomains()
        {
            var criteria = new GetLicenseeDomainCriteria
            {
                LicenseeId = 0,
                Environment = AppSettings.Environment
            };

            return GetEntites<LicenseeDomain>(criteria);
        }

        public IEnumerable<Product> GetProducts(int LicenseeID)
        {
            var criteria = new GetProductCriteria
            {
                LicenseeID = LicenseeID
            };

            return GetEntites<Product>(criteria);
        }
    }
}