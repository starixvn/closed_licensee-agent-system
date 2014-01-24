// --------------------------------------------------------------------------
// <copyright file="UnderMaintenanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Services
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using Fanex.Caching;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using Newtonsoft.Json;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UnderMaintenanceService : ServiceBase, IDisposable, IUnderMaintenanceService
    {
        private const int WaitInterval = 1000; // 1 second.
        private const string MaintenanceCacheKey = "Las.Maintenance"; // Cache Key.
        private readonly Mutex _threadMuText = new Mutex();
        private readonly ICache _cacheManager;
        private readonly HttpClient _client;

        public UnderMaintenanceService()
        {
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
            _client = new HttpClient();
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

                if (_client != null)
                {
                    _client.Dispose();
                }
            }
        }

        /// <summary>
        /// Internal Get IBC Maintenance Info.
        /// </summary>
        /// <returns>Return Under Maintenance Information.</returns>
        private UnderMaintenanceInfo InternalGetUnderMaintenanceInfo()
        {
            var result = new UnderMaintenanceInfo()
            {
                Status = false
            };

            try
            {
                // Check downtime from AppSetting.
                if (AppSettings.IsDownTime)
                {
                    return new UnderMaintenanceInfo
                    {
                        Status = true,
                        StartTime = AppSettings.BeginTime,
                        EndTime = AppSettings.EndTime
                    };
                }

                // Check downtime from library.
                if (!AppSettings.DisabledUnderMaintenanceAPI)
                {
                    HttpResponseMessage response = _client.GetAsync(AppSettings.UnderMaintenanceUrl).Result;
                    result = JsonConvert.DeserializeObject<UnderMaintenanceInfo>(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }

            return result;
        }
    }
}