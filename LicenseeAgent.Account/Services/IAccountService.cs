// --------------------------------------------------------------------------
// <copyright file="IAccountService.cs" company="Nexcel Solutions Vietnam">
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
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Entities;
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

        IEnumerable<LicenseeEntity> GetLicenseeList(GetLicenseeListCriteria criteria);

        IEnumerable<CurrencyEntity> GetCurrencyOfLicensee(GetCurrencyOfLicenseeCriteria criteria);

        void InternalLoginAsUser(InternalLoginAsUserCriteria criteria);

        void AdminCheckInLicensee(AdminCheckInLicenseeCriteria criteria);

        void AgentReCheckInLicensee(AgentReCheckInLicenseeCriteria criteria);
    }
}