namespace LicenseeAgent.Core.Services
{
    using System;
    using System.Threading;
    using Fanex.Caching;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// UnderMaintenanceService class.
    /// </summary>
    public class UnderMaintenanceService : IDisposable, IUnderMaintenanceService
    {
        private const int WaitInterval = 1000; // 1 second.
        private const int CacheTimeout = 60; // 60 seconds.
        private const string MaintenanceCacheKey = "Las.Maintenance"; // Cache Key.
        private readonly Mutex _mutext;
        private readonly ICache _cacheManager;
        private readonly UMBusiness _umservice;

        public UnderMaintenanceService()
            : this(null, null, null)
        {
        }

        public UnderMaintenanceService(UMBusiness umservice, ICache cacheManager, Mutex mutex)
        {
            _umservice = umservice ?? new UMBusiness();
            _cacheManager = cacheManager ?? CacheService.CreateInstance(CacheType.MemoryCache);
            _mutext = mutex ?? new Mutex();
        }

        /// <summary>
        /// Gets the under maintenance information.
        /// </summary>
        /// <value>
        /// The under maintenance information.
        /// </value>
        public UnderMaintenanceInfo UnderMaintenanceInfo
        {
            get
            {
                var maintenanceInfo = _cacheManager.Get<UnderMaintenanceInfo>(MaintenanceCacheKey);
                if (maintenanceInfo == null)
                {
                    if (_mutext.WaitOne(WaitInterval))
                    {
                        maintenanceInfo = InternalGetUnderMaintenanceInfo();
                        _cacheManager.Set(MaintenanceCacheKey, maintenanceInfo, CacheTimeout);
                        _mutext.ReleaseMutex();
                    }
                    else
                    {
                        maintenanceInfo = new UnderMaintenanceInfo { Status = false };
                    }
                }

                return maintenanceInfo;
            }
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
                if (_mutext != null)
                {
                    _mutext.Dispose();
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
                    TimeFrom = AppSettings.BeginTime,
                    TimeTo = AppSettings.EndTime
                };
            }

            var maintenanceInfo = new UnderMaintenanceInfo
            {
                Status = false
            };

            if (string.IsNullOrWhiteSpace(AppSettings.UnderMaintenanceUrl))
            {
                return maintenanceInfo;
            }

            var dateFrom = maintenanceInfo.TimeFrom;
            var dateTo = maintenanceInfo.TimeTo;
            maintenanceInfo.Status = _umservice.isUnderMaintainence(
                Sites.LRF,
                AppSettings.UnderMaintenanceUrl,
                ref dateFrom,
                ref dateTo);

            maintenanceInfo.TimeFrom = dateFrom;
            maintenanceInfo.TimeTo = dateTo;

            return maintenanceInfo;
        }
    }
}