// --------------------------------------------------------------------------
// <copyright file="InternalService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Account.Services
{
    using System.Web;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;

    public class InternalService : QueryService, IInternalService
    {
        private static readonly byte[] AesKey =
        {
            123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37,
            112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209
        };

        private static readonly byte[] AesVector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156 };

        public string Encode(InternalLoginInfo internalLoginInfo)
        {
            using (var rijndaelHelper = new RijndaelHelper(AesKey, AesVector))
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
            using (var rijndaelHelper = new RijndaelHelper(AesKey, AesVector))
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