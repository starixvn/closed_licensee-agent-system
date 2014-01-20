// --------------------------------------------------------------------------
// <copyright file="Integration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.API.LRF
{
    using System.Web;
    using LicenseeAgent.API.LRF.Entities;
    using LicenseeAgent.API.LRF.Services;
    using LicenseeAgent.API.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Integration
    {
        private static byte[] aesKey =
        {
            123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37,
            112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209
        };

        private static byte[] aesVector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156 };

        private static IDomainService _domainService = new DomainService();

        /// <summary>
        /// Generates the link.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="checkinUserId">The on user identifier.</param>
        /// <param name="licenseeId">The licensee identifier.</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// Returns URL for internal login from LRF.
        /// </returns>
        public static string GenerateLink(int userId, int checkinUserId, int licenseeId, string language)
        {
            var licenseeDomain = _domainService.GetLicenseeDomain(licenseeId);
            if (licenseeDomain == null)
            {
                return null;
            }

            var loginInfo = new LoginInfo
            {
                UserId = userId,
                OnUserId = checkinUserId,
                LicenseeId = licenseeId,
                Language = language
            };

            using (var rijndaelHelper = new RijndaelHelper(aesKey, aesVector))
            {
                var authKey = HttpUtility.UrlEncode(rijndaelHelper.EncryptObject(loginInfo));
                return string.Format("http://{0}/internal/lrf?authkey={1}", licenseeDomain.DomainName, authKey);
            }
        }

        public static LoginInfo Decrypt(string authKey)
        {
            using (var rijndaelHelper = new RijndaelHelper(aesKey, aesVector))
            {
                return rijndaelHelper.DecryptObject<LoginInfo>(authKey);
            }
        }
    }
}