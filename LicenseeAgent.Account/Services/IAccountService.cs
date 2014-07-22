namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Core.Criteria;
    using LicenseeAgent.Core.Entities;

    public interface IAccountService
    {
        AgentInfo AuthenticateAgent(AgentLoginCriteria criteria);

        AdminInfo AuthenticateAdmin(AdminLoginCriteria criteria);

        string GetAgentSalt(AgentSaltSelCriteria criteria);

        string GetAdminSalt(AdminSaltSelCriteria criteria);

        void AgentCheckIn(AgentCheckInCriteria criteria);

        void AdminCheckIn(AdminCheckInCriteria criteria);

        void AgentChangePassword(AgentChangePwdCriteria criteria);

        void AgentReCheckIn(AgentReCheckInCriteria criteria);

        void AdminReCheckIn(AdminReCheckInCriteria criteria);

        void AgentUserLogout(AgentUserLogoutCriteria criteria);

        void AdminUserLogout(AdminUserLogoutCriteria criteria);

        IList<LicenseeEntity> GetLicenseeList(int userId);

        IList<CurrencyEntity> GetCurrencyOfLicensee(GetCurrencyOfLicenseeCriteria criteria);

        void InternalLoginAsUser(InternalLoginAsUserCriteria criteria);

        void AdminCheckInLicensee(AdminCheckInLicenseeCriteria criteria);

        void AgentReCheckInLicensee(AgentReCheckInLicenseeCriteria criteria);

        void LicenseeAdminLogin(LicenseeAdminLoginCriteria criteria);

        IList<TopWinningEntity> GetTopWinning(TopWinningCriteria criteria);

        IList<TopTurnoverEntity> GetTopTurnover(TopTurnoverCriteria criteria);

        IList<NewMemberOfMonthEntity> GetNewMemberOfMonth(NewMemberOfMonthCriteria criteria);

        IList<StatisticEntity> GetStatisticInfo(StatisticCriteria criteria);

        IList<BalanceInfoEntity> GetBalanceInfo(BalanceInfoCriteria criteria);
    }
}