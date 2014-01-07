// --------------------------------------------------------------------------
// <copyright file="Integration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.API
{
    using LicenseeAgent.API.LRF.Entities;
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Integration
    {
        /// <summary>
        /// Generates the link.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="licenseeId">The licensee identifier.</param>
        /// <returns>Returns URL for internal login from LRF.</returns>
        public static string GenerateLink(int userId, int licenseeId)
        {
            var licenseeDomain = DomainMapper.GetDomain(licenseeId);
            if (string.IsNullOrEmpty(licenseeDomain))
            {
                return null;
            }

            var loginInfo = new LoginInfo
            {
                UserId = userId,
                LicenseeId = licenseeId
            };

            return loginInfo.ToString();
        }
    }
}