// -----------------------------------------------------------------------
// <copyright file="IUnderMaintenanceService.cs" company="Nexcel">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Services
{
    using LicenseeAgent.Core.Entities;

    public interface IUnderMaintenanceService
    {
        UnderMaintenanceInfo GetUnderMaintenanceInfo();

        void Dispose();
    }
}