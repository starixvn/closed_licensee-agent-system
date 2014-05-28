// --------------------------------------------------------------------------
// <copyright file="IAuthorizeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Web.Criteria;

    public interface IAuthorizeService
    {
        void CheckCurrencyPermission(CheckCurrencyPermissionCriteria criteria);
    }
}