// --------------------------------------------------------------------------
// <copyright file="IAuthorizeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Web.Criterias;

    public interface IAuthorizeService
    {
        void CheckCurrencyPermission(CheckCurrencyPermissionCriteria criteria);
    }
}