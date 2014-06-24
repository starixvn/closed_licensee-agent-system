// --------------------------------------------------------------------------
// <copyright file="InternalService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Account.Services
{
    using System;
    using System.Web;
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.API.LRF.Entities;
    using LicenseeAgent.API.Utility;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// InternalService class.
    /// </summary>
    public class InternalService : QueryService, IInternalService
    {
        public bool IsExpired(LoginInfo loginInfo)
        {
            return DateTime.Now.AddMinutes(Constants.LrfAuthMinuteTimeout).ToUniversalTime() > loginInfo.CreatedTime;
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
        public AgentInfo AgentInternalLogin(AgentInternalLoginCriteria criteria)
        {
            AgentInfo result = null;
            Execute(criteria);
            if (criteria.ErrCode == Result.SuccessValue)
            {
                result = new AgentInfo
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
                    LicenseeName = criteria.LicenseeName
                };
            }

            return result;
        }
    }
}