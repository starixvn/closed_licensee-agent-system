namespace LicenseeAgent.API.LRF
{
    using System.Web;
    using LicenseeAgent.API.LRF.Entities;
    using LicenseeAgent.API.LRF.Services;
    using LicenseeAgent.API.Utility;

    /// <summary>
    /// Integration class.
    /// </summary>
    public static class Integration
    {
        private static IDomainService _domainService = new DomainService();

        /// <summary>
        /// Generates the link.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="checkinUserId">The on user identifier.</param>
        /// <param name="licenseeId">The licensee identifier.</param>
        /// <param name="isInternal">If set to <c>true</c> [is internal].</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// Returns URL for internal login from LRF.
        /// </returns>
        public static string GenerateLink(int userId, int checkinUserId, int licenseeId, bool isInternal, string language)
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

            var authKey = Encrypt(loginInfo);
            var directUrl = isInternal
                ? string.Format("http://{0}/internal/lrf?authkey={1}", licenseeDomain.DomainName, authKey)
                : string.Format("http://{0}/internal/lrf?authkey={1}", Configuration.LegendAdminSite, authKey);
            return directUrl;
        }

        public static string Encrypt(LoginInfo loginInfo)
        {
            using (var rijndaelHelper = new RijndaelHelper())
            {
                return HttpUtility.UrlEncode(rijndaelHelper.EncryptObject(loginInfo));
            }
        }

        public static LoginInfo Decrypt(string authKey)
        {
            using (var rijndaelHelper = new RijndaelHelper())
            {
                return rijndaelHelper.DecryptObject<LoginInfo>(authKey);
            }
        }
    }
}