// -----------------------------------------------------------------------
// <copyright file="IUnderMaintenanceService.cs" company="Nexcel">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Services
{
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IUnderMaintenanceService
    {
        UnderMaintenanceInfo GetUnderMaintenanceInfo();

        void Dispose();
    }
}