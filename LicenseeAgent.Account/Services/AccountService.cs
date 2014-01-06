// --------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.8">Created</change>
//   <change who="Lewis.Le" date="2013.4.18">Added GetCommissionToMember, GetCommissionToAgent</change>
// </history>

namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using Fanex.Common;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// Implements service functions related with account.
    /// </summary>
    public class AccountService : QueryService, IAccountService
    {
        /// <summary>
        /// Authenticates the agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns agent information.</returns>
        public AgentInfo AuthenticateAgent(AgentLoginCriteria criteria)
        {
            var result = new AgentInfo();
            criteria.UserPwd = Cryptography.CreateHash(criteria.UserPwd, criteria.UserSalt);
            Execute(criteria);
            if (criteria.ErrCode == 0)
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
                    LastLoggedTime = criteria.LastLoggedTime
                };
            }

            return result;
        }

        /// <summary>
        /// Authenticates the admin.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return admin information.</returns>
        public AdminInfo AuthenticateAdmin(AdminLoginCriteria criteria)
        {
            var result = new AdminInfo();
            criteria.UserPwd = Cryptography.CreateHash(criteria.UserPwd, criteria.UserSalt);
            Execute(criteria);
            if (criteria.MsgId == 0)
            {
                result = new AdminInfo
                {
                    UserId = criteria.UserId,
                    UserName = criteria.UserName
                };
            }

            return result;
        }

        public string GetAgentSalt(AgentSaltSelCriteria criteria)
        {
            Execute(criteria);
            return criteria.UserSalt;
        }

        public string GetAdminSalt(AdminSaltSelCriteria criteria)
        {
            Execute(criteria);
            return criteria.UserSalt;
        }

        public void AdminCheckIn(AdminCheckInCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentCheckIn(AgentCheckInCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentChangePassword(AgentChangePwdCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentReCheckIn(AgentReCheckInCriteria criteria)
        {
            Execute(criteria);
        }

        public void AdminReCheckIn(AdminReCheckInCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentUserLogout(AgentUserLogoutCriteria criteria)
        {
            Execute(criteria);
        }

        public void AdminUserLogout(AdminUserLogoutCriteria criteria)
        {
            Execute(criteria);
        }

        public IEnumerable<LicenseeEntity> GetLicenseeList(GetLicenseeListCriteria criteria)
        {
            return GetEntites<LicenseeEntity>(criteria);
        }

        public IEnumerable<CurrencyEntity> GetCurrencyOfLicensee(GetCurrencyOfLicenseeCriteria criteria)
        {
            return GetEntites<CurrencyEntity>(criteria);
        }

        public void InternalLoginAsUser(InternalLoginAsUserCriteria criteria)
        {
            Execute(criteria);
        }

        public void AdminCheckInLicensee(AdminCheckInLicenseeCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentReCheckInLicensee(AgentReCheckInLicenseeCriteria criteria)
        {
            Execute(criteria);
        }
    }
}