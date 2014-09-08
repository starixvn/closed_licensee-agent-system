namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Account.Criteria.Internal;
    using LicenseeAgent.Account.Entities.Internal;
    using LicenseeAgent.Core.Entities;

    public interface IInternalService
    {
        IList<LicenseeEntity> GetLicensees(int userId);

        IList<CurrencyEntity> GetLicenseeCurrencies(GetCurrencyOfLicenseeCriteria criteria);

        string GetAdminSalt(AdminSaltSelectCriteria criteria);

        AdminInfo AuthenticateAdmin(AdminLoginCriteria criteria);

        void LicenseeLogin(LicenseeAdminLoginCriteria criteria);

        void AdminCheckIn(AdminCheckInCriteria criteria);

        void AdminCheckInLicensee(AdminCheckInLicenseeCriteria criteria);

        void InternalLoginAsAgent(InternalLoginAsUserCriteria criteria);

        AgentInfo InternalGetAgentInfo(InternalGetAgentInfoCriteria criteria);

        string Encode(InternalLoginInfo internalLoginInfo);

        InternalLoginInfo Decode(string authKey);

        void AdminReCheckIn(AdminReCheckInCriteria criteria);

        void AdminUserLogout(AdminUserLogoutCriteria criteria);
    }
}