namespace LicenseeAgent.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.Caching;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Criteria;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// CoreService class.
    /// </summary>
    public class CoreService : QueryService, ICoreService
    {
        private const string BalanceUpDayCacheKey = "LAS_BalanceUpDayCacheKey";
        private const string LicenseDomainsCacheKey = "LAS_LICENSEE_DOMAINS";
        private const int LicenseeDomainsCacheTimeout = 30 * 60; // 60 minutes

        private readonly ICache _cache;

        public CoreService()
            : this(null)
        {
        }

        public CoreService(ICache cache)
        {
            _cache = cache ?? CacheService.CreateInstance();
        }

        /// <summary>
        /// Gets the balance up day.
        /// </summary>
        /// <value>
        /// The balance up day.
        /// </value>
        public SelBalanceUpDay BalanceUpDay
        {
            get
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
        }

        /// <summary>
        /// Gets the licensee domains.
        /// </summary>
        /// <value>
        /// The licensee domains.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Licensee domains configuration is fail.</exception>
        public IList<LicenseeDomain> LicenseeDomains
        {
            get
            {
                var licenseeDomains = _cache.Get<IList<LicenseeDomain>>(LicenseDomainsCacheKey);
                if (licenseeDomains == null)
                {
                    licenseeDomains = InternalGetLicenseeDomains();
                    if (licenseeDomains == null || !licenseeDomains.Any())
                    {
                        throw new InvalidOperationException("Licensee domains configuration is fail.");
                    }

                    _cache.Set(LicenseDomainsCacheKey, licenseeDomains, LicenseeDomainsCacheTimeout);
                }

                return licenseeDomains;
            }
        }

        /// <summary>
        /// Gets licensee id by URL.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Returns licensee id.
        /// </returns>
        public int? GetLicenseeId(HttpRequestBase request)
        {
            var selectedLicenseeDomains = LicenseeDomains.Where(x => x.DomainName.IndexOf(request.Url.Host, StringComparison.OrdinalIgnoreCase) != -1
                && string.Compare(x.Environment, AppSettings.DomainEnvironment, StringComparison.OrdinalIgnoreCase) == 0).ToList();
            if (selectedLicenseeDomains.Count() == 1)
            {
                return selectedLicenseeDomains.First().LicenseeId;
            }

            if (AppSettings.IsLocalRequest)
            {
                return (int)Licensee.Licensee_27;
            }

            return null;
        }

        /// <summary>
        /// Gets the licensee identifier.
        /// </summary>
        /// <param name="licenseeName">Name of the licensee.</param>
        /// <returns>Returns licensee id.</returns>
        public int? GetLicenseeId(string licenseeName)
        {
            var selectedLicenseeDomains = LicenseeDomains.Where(x => x.LicenseeName.Equals(licenseeName, StringComparison.OrdinalIgnoreCase)
                && string.Compare(x.Environment, AppSettings.DomainEnvironment, StringComparison.OrdinalIgnoreCase) == 0).ToList();

            if (selectedLicenseeDomains.Count() == 1)
            {
                return selectedLicenseeDomains.First().LicenseeId;
            }

            if (AppSettings.IsLocalRequest)
            {
                return (int)Licensee.Licensee_27;
            }

            return null;
        }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <param name="licenseeId">The licensee identifier.</param>
        /// <returns>Returns list of offered products.</returns>
        public IList<ProductInfo> GetProducts(int licenseeId)
        {
            var criteria = new GetProductCriteria
            {
                LicenseeId = licenseeId
            };

            var products = GetEntites<ProductInfo>(criteria);
            if (AppSettings.OfferedProducts != "*")
            {
                var offeredProducts = AppSettings.OfferedProducts.Split(',');
                products = products.Where(x => offeredProducts.Contains(x.ProductId.ToString())).ToList();
            }

            return products;
        }

        /// <summary>
        /// Has product offered.
        /// </summary>
        /// <param name="licenseeId">ID of Licensee.</param>
        /// <param name="productId">ID of Product.</param>
        /// <returns>If has product offered return true, else return false.</returns>
        public bool HasProductOffered(int licenseeId, int productId)
        {
            var productInfos = GetProducts(licenseeId);
            return HasProductOffered(productInfos, productId);
        }

        /// <summary>
        /// Has product offered.
        /// </summary>
        /// <param name="productInfos">Product Information.</param>
        /// <param name="productId">ID of Product.</param>
        /// <returns>If has product offered return true, else return false.</returns>
        public bool HasProductOffered(IList<ProductInfo> productInfos, int productId)
        {
            return productInfos.Where(x => x.ProductId == productId).Count() == 1;
        }

        /// <summary>
        /// Get agent information.
        /// </summary>
        /// <param name="agentId">Agent ID for get agent information.</param>
        /// <returns>Agent Information.</returns>
        public AgentInfo GetAgentInfo(int agentId)
        {
            var criteria = new GetAgentInfoCriteria
            {
                AgentId = agentId
            };

            return GetEntity<AgentInfo>(criteria);
        }

        /// <summary>
        /// Gets the licensee domains. If licenseeId=0 then get all licensee domains.
        /// </summary>
        /// <returns>Returns list of licensee domain.</returns>
        private IList<LicenseeDomain> InternalGetLicenseeDomains()
        {
            var criteria = new GetLicenseeDomainCriteria
            {
                LicenseeId = 0,
                Environment = AppSettings.DomainEnvironment
            };

            return GetEntites<LicenseeDomain>(criteria);
        }
    }
}