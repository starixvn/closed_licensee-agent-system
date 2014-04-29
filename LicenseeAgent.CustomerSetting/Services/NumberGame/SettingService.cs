// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services.NumberGame
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria.NumberGame;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Services;
    using LicenseeAgent.CustomerSetting.Utils;

    /// <summary>
    /// SettingRepository class.
    /// </summary>
    public class SettingService : SettingServiceBase, ISettingService
    {
        /// <summary>
        /// Get Agent setting for insert new.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>
        /// Number game customer setting.
        /// </returns>
        public CustomerSetting2 GetSettingForInsert(SettingContext settingContext, out int errCode)
        {
            CustomerSetting customerSetting;
            errCode = Result.FailureValue;
            var maxLicenseePt = 0;
            GetSettingForInsert(
                settingContext,
                out customerSetting,
                out errCode,
                out maxLicenseePt);

            if (errCode == Result.SuccessValue)
            {
                return new CustomerSetting2
                {
                    CommissionSetting = customerSetting.CommissionSetting,
                    BettingLimitSetting = customerSetting.BettingLimitSetting,
                    PositionTakingSetting = BuildPositionTakingGroups((int)Product.Product_2, settingContext, customerSetting.PositionTakingSetting, maxLicenseePt)
                };
            }

            return null;
        }

        /// <summary>
        /// Get Insert Setting Criteria.
        /// </summary>
        /// <param name="settingContext">Setting Context.</param>
        /// <returns>Insert Criteria.</returns>
        private ICriteria GetInsertSettingCriteria(SettingContext settingContext)
        {
            if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                return new LicenseeGetAgentSettingForInsertCriteria
                {
                    AgentL0Id = settingContext.CustId,
                };
            }

            return new AgentGetAgentSettingForInsertCriteria
            {
                AgentId = settingContext.CustId
            };
        }

        /// <summary>
        /// Get default setting for insert new agent.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="errCode">The MSG unique identifier.</param>
        /// <param name="maxLicenseePt">Max PT in case insert agent level 1.</param>
        private void GetSettingForInsert(
            SettingContext settingContext,
            out CustomerSetting customerSetting,
            out int errCode,
            out int maxLicenseePt)
        {
            var criteria = GetInsertSettingCriteria(settingContext);
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                maxLicenseePt = 0;
                errCode = Result.FailureValue;
                var commissionSetting = new CommissionSetting();
                IList<BettingLimitItem> bettingLimitList = null;
                IList<PositionTakingItem> positionTakingList = null;
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        commissionSetting.CommissionItems = CommissionDataReader.ReadNumberGameAgentCommissionData(reader, false);
                        if (reader.NextResult())
                        {
                            bettingLimitList = BettingLimitDataReader.ReadNumberGameBetLimitData(reader, false);
                        }

                        if (settingContext.TargetRoleId != (int)Role.AgentLevel1 && reader.NextResult())
                        {
                            positionTakingList = PositionTakingDataReader.ReadAgentPositionTakingData(reader, false);
                        }
                    }

                    var outputParams = objDb.GetOuputParams();
                    errCode = Converter.ToInt(outputParams["msgid"]);
                    if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
                    {
                        maxLicenseePt = Converter.ToInt(outputParams["maxptoflicensee"]) / Constant.STEP_MUL_DIV;
                    }
                }

                customerSetting = new CustomerSetting
                {
                    CommissionSetting = commissionSetting,
                    BettingLimitSetting = bettingLimitList,
                    PositionTakingSetting = positionTakingList
                };
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }
    }
}