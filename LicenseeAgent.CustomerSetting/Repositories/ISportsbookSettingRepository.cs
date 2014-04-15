// --------------------------------------------------------------------------
// <copyright file="ISportsBookSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.SportsBook;

    public interface ISportsBookSettingRepository
    {
        Result UpdateSetting(ICriteria criteria, ICriteria logCriteria);

        Dictionary<string, PositionTaking> GetCustomerPositionTakingSettingForUpdating(ICriteria criteria);

        Dictionary<string, PositionTaking> GetAgentPositionTakingSettingForUpdating(ICriteria criteria);

        IList<BettingLimit> GetBettingLimitSettingForUpdating(ICriteria criteria);

        IList<CommissionItem> GetAgentCommissionSetingForUpdating(ICriteria criteria);

        IList<CommissionItem> GetCustomerCommissionSetingForUpdating(ICriteria criteria);

        MemberInfo GetMemberInfo(ICriteria criteria);

        InsertResult InsertCustomer(ICriteria criteria);

        InsertResult UpdateCustomer(ICriteria criteria);

        InsertResult UpdateIndirectCustomer(ICriteria criteria);

        void GetSettingForInsert(
            ICriteria criteria,
            SettingContext settingContext,
            out CustomerInfo custInfo,
            out CommissionSetting commissionItems,
            out IList<BettingLimit> bettingLimits,
            out Dictionary<string, PositionTaking> positionTakingDataList,
            out int msgId,
            out int maxLicenseePt);

        void GetSettingForUpdate(
            ICriteria criteria,
            SettingContext settingContext,
            out CustomerInfo custInfo,
            out CommissionSetting commissionItems,
            out IList<BettingLimit> bettingLimits,
            out Dictionary<string, PositionTaking> positionTakingDataList,
            out int msgId);
    }
}