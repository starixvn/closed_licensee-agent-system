// --------------------------------------------------------------------------
// <copyright file="IAccountService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Core.Criterias;
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

        IEnumerable<LicenseeEntity> GetLicenseeList(GetLicenseeListCriteria criteria);

        IEnumerable<CurrencyEntity> GetCurrencyOfLicensee(GetCurrencyOfLicenseeCriteria criteria);

        void InternalLoginAsUser(InternalLoginAsUserCriteria criteria);

        void AdminCheckInLicensee(AdminCheckInLicenseeCriteria criteria);

        void AgentReCheckInLicensee(AgentReCheckInLicenseeCriteria criteria);

        void LicenseeAdminLogin(LicenseeAdminLoginCriteria criteria);

        IEnumerable<TopWinningEntity> GetTopWinning(TopWinningCriteria criteria);

        IEnumerable<TopTurnoverEntity> GetTopTurnover(TopTurnoverCriteria criteria);

        IEnumerable<NewMemberOfMonthEntity> GetNewMemberOfMonth(NewMemberOfMonthCriteria criteria);

        IEnumerable<StatisticEntity> GetStatisticInfo(StatisticCriteria criteria);

        IEnumerable<BalanceInfoEntity> GetBalanceInfo(BalanceInfoCriteria criteria);
    }
}