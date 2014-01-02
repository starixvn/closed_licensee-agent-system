// --------------------------------------------------------------------------
// <copyright file="ICoreService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace LicenseeAgent.Core.Services
{
    using System;
    using LicenseeAgent.Core.Entities;

    public interface ICoreService
    {
        SelBalanceUpDay GetBalanceUpDay();
    }
}
