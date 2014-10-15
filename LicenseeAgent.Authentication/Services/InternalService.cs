namespace LicenseeAgent.Authentication.Services
{
    using System;
    using System.Web;
    using LicenseeAgent.API.Utility;
    using LicenseeAgent.Authentication.Criteria;
    using LicenseeAgent.Authentication.Criteria.Internal;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// InternalService class.
    /// </summary>
    public class InternalService : QueryService, IInternalService
    {
        public void LicenseeLogin(LicenseeAdminLoginCriteria criteria)
        {
            Execute(criteria);
        }

        public void LicenseeCheckIn(AdminCheckInCriteria criteria)
        {
            Execute(criteria);
        }

        public void AdminCheckInLicensee(AdminCheckInLicenseeCriteria criteria)
        {
            Execute(criteria);
        }

        public string Encode(InternalLoginInfo internalLoginInfo)
        {
            using (var rijndaelHelper = new RijndaelHelper())
            {
                var authKey = HttpUtility.UrlEncode(rijndaelHelper.EncryptObject(internalLoginInfo));
                return authKey;
            }
        }

        /// <summary>
        /// Decodes the specified authentication key.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>Returns internal login information.</returns>
        public InternalLoginInfo Decode(string authKey)
        {
            using (var rijndaelHelper = new RijndaelHelper())
            {
                return rijndaelHelper.DecryptObject<InternalLoginInfo>(authKey);
            }
        }

        /// <summary>
        /// Agents the internal login.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns agent information.</returns>
        public AgentInfo InternalGetAgentInfo(InternalGetAgentInfoCriteria criteria)
        {
            Execute(criteria);
            if (criteria.ErrCode == Result.SuccessValue)
            {
                return new AgentInfo
                {
                    UserName = criteria.UserName,
                    UserSalt = criteria.UserSalt,
                    AgentId = criteria.AgentId,
                    AgentLevel = criteria.AgentLevel,
                    NumberOfLevel = criteria.NumberOfLevel,
                    SubAccId = criteria.SubAccId,
                    CurrencyName = criteria.Currency,
                    CurrencyId = criteria.CurrencyId,
                    IsFirstLogin = criteria.IsFirstLogin,
                    PwdExpiry = criteria.PwdExpiry,
                    LastChangedPwdDate = criteria.LastChangedPwdDate,
                    LicenseeId = criteria.LicenseeId,
                    IsNickName = criteria.IsNickName,
                    LastLoggedTime = criteria.LastLoggedTime,
                    LicenseeName = criteria.LicenseeName,
                    LegendType = (LegendType)criteria.LegendType,
                    IsAllowedMemberComm = criteria.IsAllowedMemberComm
                };
            }
            else
            {
                throw new InvalidOperationException(ResourceManager.GetErrMsg(criteria.ErrCode));
            }
        }

        public void InternalLoginAsAgent(InternalLoginAsUserCriteria criteria)
        {
            Execute(criteria);
        }
    }
}