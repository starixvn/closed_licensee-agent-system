namespace LicenseeAgent.API.LRF
{
    using System;
    using System.Web;
    using LicenseeAgent.API.LRF.Criteria;
    using LicenseeAgent.API.LRF.Results;
    using LicenseeAgent.API.LRF.Services;
    using LicenseeAgent.API.Utility;

    /// <summary>
    /// Integration class.
    /// </summary>
    public class Integration
    {
        #region Variables

        private readonly IAuthorityService _authorityService;

        #endregion Variables

        #region Constructors

        public Integration(IAuthorityService authorityService, string legendEnvironment, string legendAdminSite)
        {
            _authorityService = authorityService ?? new AuthorityService();
            LegendEnvironment = legendEnvironment ?? Configuration.LegendEnvironment;
            LegendAdminSite = legendAdminSite ?? Configuration.LegendAdminSite;
        }

        public Integration()
            : this(null, null, null)
        {
        }

        #endregion Constructors

        #region Properties

        public string LegendEnvironment { get; private set; }

        public string LegendAdminSite { get; private set; }

        #endregion Properties

        #region Generate link

        /// <summary>
        /// Generates the link.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="checkInUserId">The on user identifier.</param>
        /// <param name="licenseeId">The licensee identifier.</param>
        /// <param name="isInternal">If set to <c>true</c> [is internal].</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// Returns URL for internal login from LRF.
        /// </returns>
        public string GenerateLink(int userId, int checkInUserId, int licenseeId, bool isInternal, string language)
        {
            var httpRequest = HttpContext.Current.Request;
            var httpRequestBase = new HttpRequestWrapper(httpRequest);
            var clientInfo = httpRequestBase.GetClientInfo();
            var criteria = new InsertInternalAuthorityCriteria
            {
                LicenseeId = licenseeId,
                UserId = userId,
                OnUserId = checkInUserId,
                Language = language,
                Environment = LegendEnvironment,
                Ip = clientInfo.Ip,
                Browser = clientInfo.BrowserName
            };

            _authorityService.InsertInternalAuthority(criteria);
            if (criteria.ErrCode != 0)
            {
                throw new InvalidOperationException(criteria.ErrMsg);
            }

            var authKey = string.Empty;
            using (var rijndaelHelper = new RijndaelHelper())
            {
                authKey = HttpUtility.UrlEncode(rijndaelHelper.EncryptString(criteria.Guid.ToString()));
            }

            var link = isInternal
                ? string.Format("{0}/internal/lrf?authkey={1}", criteria.DomainName, authKey)
                : string.Format("{0}/internal/lrf?authkey={1}", LegendAdminSite, authKey);
            link = link.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? link : string.Format("http://{0}", link);
            return link;
        }

        #endregion Generate link

        #region Get link info

        /// <summary>
        /// Gets the internal information.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>
        /// Returns internal information.
        /// </returns>
        public GetInternalInfoResult GetInternalInfo(string authKey)
        {
            var guid = string.Empty;
            try
            {
                using (var rijndaelHelper = new RijndaelHelper())
                {
                    guid = HttpUtility.UrlEncode(rijndaelHelper.DecryptString(authKey));
                }
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }

            var httpRequest = HttpContext.Current.Request;
            var httpRequestBase = new HttpRequestWrapper(httpRequest);
            var clientInfo = httpRequestBase.GetClientInfo();
            var criteria = new GetInternalAuthorityCriteria
            {
                Guid = guid,
                Ip = clientInfo.Ip,
                Browser = clientInfo.BrowserName
            };

            var internalInfo = _authorityService.GetInternalAuthority(criteria);
            return new GetInternalInfoResult
            {
                ErrCode = criteria.ErrCode,
                InternalInfo = internalInfo
            };
        }

        #endregion Get link info
    }
}