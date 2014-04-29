// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services.NumberGame
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
        /// Get Agent setting for updating.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>
        /// Sports book customer setting.
        /// </returns>
        public CustomerSetting2 GetSettingForUpdate(SettingContext settingContext, out int errCode)
        {
            CustomerSetting customerSetting;
            errCode = Result.FailureValue;
            GetSettingForUpdate(
                settingContext,
                out customerSetting,
                out errCode);

            if (errCode == Result.SuccessValue)
            {
                return new CustomerSetting2
                {
                    CustInfo = customerSetting.CustInfo,
                    CommissionSetting = customerSetting.CommissionSetting,
                    BettingLimitSetting = customerSetting.BettingLimitSetting,
                    PositionTakingSetting = BuildPositionTakingGroupsForUpdate((int)Product.Product_0, customerSetting.PositionTakingSetting)
                };
            }

            return null;
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
            var criteria = GetCriteriaForInsert(settingContext);
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

        /// <summary>
        /// Get default setting for edit customer.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="errCode">The MSG unique identifier.</param>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void GetSettingForUpdate(
            SettingContext settingContext,
            out CustomerSetting customerSetting,
            out int errCode)
        {
            var criteria = GetCriteriaForUpdate(settingContext);
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                var custInfo = new CustomerInfo();
                var commissionSetting = new CommissionSetting
                {
                    UserGroup = "A"
                };
                IList<BettingLimitItem> bettingLimiList = null;
                IList<PositionTakingItem> positionTakingList = null;
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        if (settingContext.FormId != FormType.EditIndirectMember)
                        {
                            commissionSetting.CommissionItems = settingContext.TargetRoleId == (int)Role.Member
                                ? CommissionDataReader.ReadMemberCommissionData(reader)
                                : CommissionDataReader.ReadNumberGameAgentCommissionData(reader);

                            if (reader.NextResult())
                            {
                                bettingLimiList = BettingLimitDataReader.ReadBetLimitData(reader, true);
                            }
                        }

                        if (reader.NextResult())
                        {
                            positionTakingList = settingContext.TargetRoleId == (int)Role.Member
                                ? PositionTakingDataReader.ReadMemberPositionTakingData(reader)
                                : PositionTakingDataReader.ReadAgentPositionTakingDataForUpdate(reader, settingContext.TargetRoleId);
                        }
                    }

                    var outputParams = objDb.GetOuputParams();
                    errCode = Converter.ToInt(outputParams["msgid"]);
                    customerSetting = new CustomerSetting
                    {
                        CustInfo = custInfo,
                        CommissionSetting = commissionSetting,
                        BettingLimitSetting = bettingLimiList,
                        PositionTakingSetting = positionTakingList
                    };
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Get Insert Setting Criteria.
        /// </summary>
        /// <param name="settingContext">Setting Context.</param>
        /// <returns>Insert Criteria.</returns>
        private ICriteria GetCriteriaForInsert(SettingContext settingContext)
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
        /// Initializes the criteria for getting SportsBook setting for update.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Criteria for getting SportsBook setting.</returns>
        private ICriteria GetCriteriaForUpdate(SettingContext settingContext)
        {
            ICriteria criteria = null;

            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                if (settingContext.FormId == FormType.EditIndirectMember)
                {
                    criteria = new AgentGetMemberSettingIndirectForUpdateCriteria
                    {
                        AgentId = settingContext.CustId,
                        CustId = settingContext.TargetCustId
                    };
                }
                else
                {
                    criteria = new AgentGetMemberSettingForUpdateCriteria
                    {
                        CustId = settingContext.TargetCustId
                    };
                }
            }
            else if (settingContext.TargetRoleId == (int)Role.Agent)
            {
                criteria = new AgentGetAgentSettingForUpdateCriteria
                {
                    AgentId = settingContext.TargetCustId
                };
            }
            else if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                criteria = new LicenseeGetAgentSettingForUpdateCriteria
                {
                    AgentId = settingContext.TargetCustId,
                };
            }

            return criteria;
        }
    }
}