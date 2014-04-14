// -----------------------------------------------------------------------
// <copyright file="CurrencyService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Core.Services;
    
    public class CurrencyService : QueryService, ICurrencyService
    {
        /// <summary>
        /// Get currency list.
        /// </summary>
        /// <param name="criteria">List currency criteria.</param>
        /// <returns>List currency information.</returns>
        public IEnumerable<CurrencyEntity> GetCurrencyList(GetCurrencyOfLicenseeCriteria criteria)
        {
            return GetEntites<CurrencyEntity>(criteria);
        }
    }
}
