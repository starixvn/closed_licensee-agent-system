// --------------------------------------------------------------------------
// <copyright file="UnderMaintenanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Services
{
    using System;
    using System.Threading;
    using Fanex.Caching;
    using Fanex.Logging;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UnderMaintenanceService : IDisposable, IUnderMaintenanceService
    {
        private const int WaitInterval = 1000; // 1 second.
        private const string MaintenanceCacheKey = "Las.Maintenance"; // Cache Key.
        private readonly Mutex _threadMuText = new Mutex();
        private readonly ICache _cacheManager;
        private readonly UMBusiness _umservice;

        public UnderMaintenanceService()
        {
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
            _umservice = new UMBusiness();
        }

        /// <summary>
        /// Gets the under maintenance information.
        /// </summary>
        /// <returns>Returns object.</returns>
        public UnderMaintenanceInfo GetUnderMaintenanceInfo()
        {
            var maintenanceInfo = _cacheManager.Get<UnderMaintenanceInfo>(MaintenanceCacheKey);
            if (maintenanceInfo == null)
            {
                if (_threadMuText.WaitOne(WaitInterval))
                {
                    maintenanceInfo = InternalGetUnderMaintenanceInfo();
                    _cacheManager.Set<UnderMaintenanceInfo>(MaintenanceCacheKey, maintenanceInfo, TimeSpan.FromMinutes(1));
                    _threadMuText.ReleaseMutex();
                }
                else
                {
                    maintenanceInfo = new UnderMaintenanceInfo { Status = false };
                }
            }

            return maintenanceInfo;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c>
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_threadMuText != null)
                {
                    _threadMuText.Dispose();
                }
            }
        }

        /// <summary>
        /// Internal Get IBC Maintenance Info.
        /// </summary>
        /// <returns>Return Under Maintenance Information.</returns>
        private UnderMaintenanceInfo InternalGetUnderMaintenanceInfo()
        {
            if (AppSettings.IsDownTime)
            {
                return new UnderMaintenanceInfo
                {
                    Status = true,
                    UmTimeFrom = AppSettings.BeginTime,
                    UmTimeTo = AppSettings.EndTime
                };
            }

            var maintenanceInfo = new UnderMaintenanceInfo()
            {
                Status = false
            };

            if (string.IsNullOrWhiteSpace(AppSettings.UnderMaintenanceUrl))
            {
                return maintenanceInfo;
            }

            try
            {
                var dateFrom = maintenanceInfo.UmTimeFrom;
                var dateTo = maintenanceInfo.UmTimeTo;
                maintenanceInfo.Status = _umservice.isUnderMaintainence(
                    Sites.LRF,
                    AppSettings.UnderMaintenanceUrl,
                    ref dateFrom,
                    ref dateTo);

                maintenanceInfo.UmTimeFrom = dateFrom;
                maintenanceInfo.UmTimeTo = dateTo;
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
            }

            return maintenanceInfo;
        }
    }
}