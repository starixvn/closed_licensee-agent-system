// --------------------------------------------------------------------------
// <copyright file="IDomainService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.API.LRF.Services
{
    using LicenseeAgent.API.LRF.Entities;

    public interface IDomainService
    {
        LicenseeDomain GetLicenseeDomain(int licenseeId);
    }
}