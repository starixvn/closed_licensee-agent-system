// --------------------------------------------------------------------------
// <copyright file="ICoreService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Entities;

    public interface ICoreService
    {
        SelBalanceUpDay GetBalanceUpDay();

        IEnumerable<LicenseeDomain> GetLicenseeDomains();

        IEnumerable<Product> GetProducts(int licenseeId);
    }
}