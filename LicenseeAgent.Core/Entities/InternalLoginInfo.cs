// --------------------------------------------------------------------------
// <copyright file="InternalLoginInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Entities
{
    using System;
    using Fanex.Caching;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InternalLoginInfo
    {
        private const int Timeout = 60; // 1 minute
        private static ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);

        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public string AgentName { get; set; }

        public string Language { get; set; }

        public int UserId { get; set; }

        public int OnUserId { get; set; }

        /// <summary>
        /// Gets the information from cache.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>Returns internal login info.</returns>
        public static InternalLoginInfo GetInfo(string guid)
        {
            var cacheKey = GetCacheKey(guid);
            var data = cache.Get<string>(cacheKey);
            if (!string.IsNullOrEmpty(data))
            {
                var values = data.Split('|');
                if (values.Length == 6)
                {
                    return new InternalLoginInfo
                    {
                        LicenseeId = Convert.ToInt32(values[0]),
                        CurrencyId = Convert.ToInt32(values[1]),
                        AgentName = values[2],
                        Language = values[3],
                        UserId = Convert.ToInt32(values[4]),
                        OnUserId = Convert.ToInt32(values[5])
                    };
                }
            }

            return null;
        }

        public string Save()
        {
            var guid = Guid.NewGuid().ToString();
            var data = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", LicenseeId, CurrencyId, AgentName, Language, UserId, OnUserId);
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