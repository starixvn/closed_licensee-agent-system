// --------------------------------------------------------------------------
// <copyright file="AdminInternalLoginInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.10.21">Created</change>
// </history>

namespace LicenseeAgent.Account.Entities
{
    using System;
    using Fanex.Caching;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AdminInternalLoginInfo
    {
        private const int Timeout = 60; // 1 minute
        private static ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);

        public int AdminId { get; set; }

        public int OnAdminId { get; set; }

        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string UserName { get; set; }

        public string Language { get; set; }

        /// <summary>
        /// Gets the information from cache.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>Returns internal login info.</returns>
        public static AdminInternalLoginInfo GetInfo(string guid)
        {
            var cacheKey = GetCacheKey(guid);
            var data = cache.Get<string>(cacheKey);
            if (!string.IsNullOrEmpty(data))
            {
                var values = data.Split('|');
                if (values.Length == 6)
                {
                    return new AdminInternalLoginInfo
                    {
                        LicenseeId = Convert.ToInt32(values[0]),
                        CurrencyId = Convert.ToInt32(values[1]),
                        UserName = values[2],
                        Language = values[3],
                        AdminId = Convert.ToInt32(values[4]),
                        OnAdminId = Convert.ToInt32(values[5])
                    };
                }
            }

            return null;
        }

        public string Save()
        {
            var guid = Guid.NewGuid().ToString();
            var data = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", LicenseeId, CurrencyId, UserName, Language, AdminId, OnAdminId);
            var cacheKey = GetCacheKey(guid);
            cache.Set<string>(cacheKey, data, Timeout);
            return guid;
        }

        private static string GetCacheKey(string guid)
        {
            return string.Format("LAS_Internal_{0}", guid);
        }
    }
}