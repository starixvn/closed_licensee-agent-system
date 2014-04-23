// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services.NumberGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria.NumberGame.Agent;
    using LicenseeAgent.CustomerSetting.Criteria.NumberGame.Licensee;
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
        public Entities.NumberGame.CustomerSetting GetSettingForInsert(SettingContext settingContext, out int errCode)
        {
            CommissionSetting commissionSetting;
            IList<Entities.NumberGame.BettingLimit> bettingLimits;
            Dictionary<string, PositionTaking> positionTakingData;
            ICriteria getAgentSettingCriteria = GetInsertSettingCriteria(settingContext);
            errCode = Result.FailureValue;
            var maxLicenseePt = 0;

            GetSettingForInsert(
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
                    PositionTakingSetting = BuildPositionTakingGroups((int)Product.Product_2, settingContext, positionTakingData, maxLicenseePt)
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

        /// <summary>
        /// Get default setting for insert new agent.
        /// </summary>
        /// <param name="criteria">SP parameters.</param>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="commissionSetting">Commission setting.</param>
        /// <param name="bettingLimits">Bet limit setting.</param>
        /// <param name="positionTakingDataList">Position taking setting.</param>
        /// <param name="msgId">The MSG unique identifier.</param>
        /// <param name="maxLicenseePt">Max PT in case insert agent level 1.</param>
        /// <exception cref="System.Exception">Throw an invalid criteria exception.</exception>
        private void GetSettingForInsert(
            ICriteria criteria,
            SettingContext settingContext,
            out CommissionSetting commissionSetting,
            out IList<Entities.NumberGame.BettingLimit> bettingLimits,
            out Dictionary<string, PositionTaking> positionTakingDataList,
            out int msgId,
            out int maxLicenseePt)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                maxLicenseePt = 0;
                msgId = -1;
                commissionSetting = new CommissionSetting();
                bettingLimits = null;
                positionTakingDataList = null;
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        commissionSetting.CommissionItems = CommissionDataReader.ReadNumberGameAgentCommissionData(reader, false);

                        if (reader.NextResult())
                        {
                            bettingLimits = BettingLimitDataReader.ReadNumberGameBetLimitData(reader, false);
                        }

                        if (settingContext.TargetRoleId != (int)Role.AgentLevel1)
                        {
                            if (reader.NextResult())
                            {
                                positionTakingDataList = PositionTakingDataReader.ReadAgentPositionTakingData(reader, false);
                            }
                        }
                    }

                    var outputParams = objDb.GetOuputParams();
                    msgId = Converter.ToInt(outputParams["msgid"]);
                    if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
                    {
                        maxLicenseePt = Converter.ToInt(outputParams["maxptoflicensee"]) / Constant.STEP_MUL_DIV;
                    }
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }
    }
}