namespace LicenseeAgent.Account.Services
{
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.Account.Criteria.Account;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
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

            return null;
        }

        public string GetAgentSalt(AgentSaltSelectCriteria criteria)
        {
            Execute(criteria);
            return criteria.UserSalt;
        }

        public void AgentCheckIn(AgentCheckInCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentChangePassword(AgentChangePasswordCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentReCheckIn(AgentReCheckInCriteria criteria)
        {
            Execute(criteria);
        }

        public void AgentUserLogout(AgentUserLogoutCriteria criteria)
        {
            Execute(criteria);
        }
    }
}