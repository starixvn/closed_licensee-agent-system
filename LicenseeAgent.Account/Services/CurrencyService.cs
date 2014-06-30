namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// CurrencyService class.
    /// </summary>
    public class CurrencyService : QueryService, ICurrencyService
    {
        /// <summary>
        /// Get currency list.
        /// </summary>
        /// <param name="criteria">List currency criteria.</param>
        /// <returns>List currency information.</returns>
        public IList<CurrencyEntity> GetCurrencyList(GetCurrencyOfLicenseeCriteria criteria)
        {
            return GetEntites<CurrencyEntity>(criteria);
        }
    }
}