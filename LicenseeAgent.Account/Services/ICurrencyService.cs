// -----------------------------------------------------------------------
// <copyright file="ICurrencyService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICurrencyService
    {
        /// <summary>
        /// Get currency list.
        /// </summary>
        /// <param name="criteria">List currency criteria.</param>
        /// <returns>List currency information.</returns>
        IEnumerable<CurrencyEntity> GetCurrencyList(GetCurrencyOfLicenseeCriteria criteria);
    }
}
