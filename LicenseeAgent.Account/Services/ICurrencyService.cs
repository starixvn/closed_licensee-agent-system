namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.Account.Entities;

    public interface ICurrencyService
    {
        /// <summary>
        /// Get currency list.
        /// </summary>
        /// <param name="criteria">List currency criteria.</param>
        /// <returns>List currency information.</returns>
        IList<CurrencyEntity> GetCurrencyList(GetCurrencyOfLicenseeCriteria criteria);
    }
}