namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using Fanex.Common;
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Core.Criteria;
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
            if (criteria.ErrCode == Result.SuccessValue)
            {
                result = new AdminInfo
                {
                    UserId = criteria.UserId,
                    UserName = criteria.UserName,
                    UserRole = criteria.UserRole
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

        public IList<LicenseeEntity> GetLicenseeList(GetLicenseeListCriteria criteria)
        {
            return GetEntites<LicenseeEntity>(criteria);
        }

        public IList<CurrencyEntity> GetCurrencyOfLicensee(GetCurrencyOfLicenseeCriteria criteria)
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

        public void LicenseeAdminLogin(LicenseeAdminLoginCriteria criteria)
        {
            Execute(criteria);
        }

        public IList<TopWinningEntity> GetTopWinning(TopWinningCriteria criteria)
        {
            return GetEntites<TopWinningEntity>(criteria);
        }

        public IList<TopTurnoverEntity> GetTopTurnover(TopTurnoverCriteria criteria)
        {
            return GetEntites<TopTurnoverEntity>(criteria);
        }

        public IList<NewMemberOfMonthEntity> GetNewMemberOfMonth(NewMemberOfMonthCriteria criteria)
        {
            return GetEntites<NewMemberOfMonthEntity>(criteria);
        }

        public IList<StatisticEntity> GetStatisticInfo(StatisticCriteria criteria)
        {
            return GetEntites<StatisticEntity>(criteria);
        }

        public IList<BalanceInfoEntity> GetBalanceInfo(BalanceInfoCriteria criteria)
        {
            return GetEntites<BalanceInfoEntity>(criteria);
        }
    }
}