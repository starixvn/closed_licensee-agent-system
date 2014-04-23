// ----------------------------------------------------------------------
// <copyright file="NumberGameSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Criteria.NumberGame.Agent;
    using LicenseeAgent.CustomerSetting.Criteria.NumberGame.Licensee;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Repositories;

    /// <summary>
    /// NumberGameSettingService class.
    /// </summary>
    public class NumberGameSettingService : INumberGameSettingService
    {
        private readonly INumberGameSettingRepository _numberGameSettingRepository;

        public NumberGameSettingService(INumberGameSettingRepository numberGameSettingRepository)
        {
            _numberGameSettingRepository = numberGameSettingRepository;
        }

        /// <summary>
        /// Get Agent setting for insert new.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>
        /// Number game customer setting.
        /// </returns>
        public Entities.NumberGame.CustomerSetting GetNumberGameSettingForInsert(SettingContext settingContext, out int errCode)
        {
            CommissionSetting commissionSetting;
            IList<Entities.NumberGame.BettingLimit> bettingLimits;
            Dictionary<string, PositionTaking> positionTakingData;

            ICriteria getAgentSettingCriteria = GetInsertSettingCriteria(settingContext);

            errCode = Result.FailureValue;
            var maxLicenseePt = 0;
            _numberGameSettingRepository.GetSettingForInsert(
                getAgentSettingCriteria,
                settingContext,
                out commissionSetting,
                out bettingLimits,
                out positionTakingData,
                out errCode,
                out maxLicenseePt);

            if (errCode == Result.SuccessValue)
            {
                return new Entities.NumberGame.CustomerSetting
                {
                    CommissionSetting = commissionSetting,
                    BettingLimits = bettingLimits.Cast<Entities.NumberGame.BettingLimit>().ToList(),
                    PositionTakingSetting = ServiceUtils.BuildPositionTakingGroups((int)Product.Product_2, settingContext, positionTakingData, maxLicenseePt)
                };
            }

            return null;
        }

        /// <summary>
        /// Get Insert Setting Criteria.
        /// </summary>
        /// <param name="settingContext">Setting Context.</param>
        /// <returns>Insert Criteria.</returns>
        private static ICriteria GetInsertSettingCriteria(SettingContext settingContext)
        {
            if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                return new LicenseeGetSettingForInsertCriteria
                {
                    AgentL0Id = settingContext.CustId,
                };
            }

            return new AgentGetSettingForInsertCriteria
            {
                AgentId = settingContext.CustId
            };
        }
    }
}