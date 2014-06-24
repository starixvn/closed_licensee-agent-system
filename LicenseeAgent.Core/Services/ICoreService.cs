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

        IList<LicenseeDomain> GetLicenseeDomains();

        IList<ProductInfo> GetProducts(int licenseeId);

        bool HasProductOffered(int licenseeId, int productId);

        bool HasProductOffered(IList<ProductInfo> productInfos, int productId);
    }
}