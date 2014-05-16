// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services.VirtualSports
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.CustomerSetting.Criteria.VirtualSports;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Utils;

    /// <summary>
    /// Customer repository.
    /// </summary>
    public class SettingService
    {
        /// <summary>
        /// Get customer setting for updating.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>
        /// Virtual Sports customer setting.
        /// </returns>
        public CustomerSetting2 GetSettingForUpdate(SettingContext settingContext, out int errCode)
        {
            CustomerSetting customerSetting;
            errCode = Result.FailureValue;
            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                GetSettingForUpdateMember(settingContext, out customerSetting, out errCode);
            }
            else
            {
                GetSettingForUpdateAgent(settingContext, out customerSetting, out errCode);
            }

            if (errCode != Result.SuccessValue)
            {
                return null;
            }

            var result = new CustomerSetting2
            {
                CustInfo = customerSetting.CustInfo,
                BettingLimitSetting = customerSetting.BettingLimitSetting,
                PositionTakingSetting = PositionTakingBuilder.BuildGroups((int)Product.Product_5, customerSetting.PositionTakingSetting)
            };

            return result;
        }

        /// <summary>
        /// Get default setting for edit customer.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="errCode">The MSG unique identifier.</param>
        private void GetSettingForUpdateAgent(
            SettingContext settingContext,
            out CustomerSetting customerSetting,
            out int errCode)
        {
            var criteria = GetCriteriaForUpdateAgent(settingContext);
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                var custInfo = new CustomerInfo();
                IList<BettingLimitItem> bettingLimiList = null;
                IList<PositionTakingItem> positionTakingList = null;
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        custInfo = CustomerInfoDataReader.ReaderAgentInfo(reader);

                        if (settingContext.FormId != FormType.EditIndirectMember)
                        {
                            if (reader.NextResult())
                            {
                                bettingLimiList = BettingLimitDataReader.ReadBetLimitData(reader, true);
                            }
                        }

                        if (reader.NextResult())
                        {
                            positionTakingList = PositionTakingDataReader.ReadAgentPositionTakingDataForUpdate(reader, settingContext.TargetRoleId);
                        }
                    }

                    var outputParams = objDb.GetOuputParams();
                    errCode = Converter.ToInt(outputParams["msgid"]);
                    customerSetting = CreateCustomerSetting(custInfo, bettingLimiList, positionTakingList);
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Get default setting for edit member.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="errCode">The MSG unique identifier.</param>
        private void GetSettingForUpdateMember(
            SettingContext settingContext,
            out CustomerSetting customerSetting,
            out int errCode)
        {
            var criteria = GetCriteriaForUpdateMember(settingContext);
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                var custInfo = new CustomerInfo();
                IList<BettingLimitItem> bettingLimiList = null;
                IList<PositionTakingItem> positionTakingList = null;
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        string userGroup;
                        custInfo = CustomerInfoDataReader.ReaderMemberInfo(reader, out userGroup);

                        if (settingContext.FormId != FormType.EditIndirectMember)
                        {
                            if (reader.NextResult())
                            {
                                bettingLimiList = BettingLimitDataReader.ReadBetLimitData(reader, true);
                            }
                        }

                        if (reader.NextResult())
                        {
                            positionTakingList = PositionTakingDataReader.ReadMemberPositionTakingData(reader);
                        }
                    }

                    var outputParams = objDb.GetOuputParams();
                    errCode = Converter.ToInt(outputParams["msgid"]);
                    customerSetting = CreateCustomerSetting(custInfo, bettingLimiList, positionTakingList);
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Initializes the criteria for getting Virtual Sports setting for update.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Criteria for getting Virtual Sports setting.</returns>
        private ICriteria GetCriteriaForUpdateAgent(SettingContext settingContext)
        {
            ICriteria criteria = null;

            if (settingContext.TargetRoleId == (int)Role.Agent)
            {
                criteria = new LicenseeGetAgentSettingForUpdateCriteria
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

        /// <summary>
        /// Initializes the criteria for getting Virtual Sports setting for update.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Criteria for getting Virtual Sports setting.</returns>
        private ICriteria GetCriteriaForUpdateMember(SettingContext settingContext)
        {
            ICriteria criteria = null;

            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                if (settingContext.FormId == FormType.EditIndirectMember)
                {
                    criteria = new LicenseeGetAgentSettingForUpdateCriteria
                    {
                        AgentId = settingContext.CustId
                    };
                }
                else
                {
                    criteria = new LicenseeGetAgentSettingForUpdateCriteria
                    {
                        AgentId = settingContext.CustId
                    };
                }
            }

            return criteria;
        }

        /// <summary>
        /// Create Customer setting Virtual Sports  for update.
        /// </summary>
        /// <param name="custInfo">The customer info.</param>
        /// <param name="bettingLimiList">The betting limit info.</param>
        /// <param name="positionTakingList">The position taking info.</param>
        /// <returns>Customer Setting Virtual Sports.</returns>
        private CustomerSetting CreateCustomerSetting(
            CustomerInfo custInfo,
            IList<BettingLimitItem> bettingLimiList,
            IList<PositionTakingItem> positionTakingList)
        {
            return new CustomerSetting
            {
                CustInfo = custInfo,
                BettingLimitSetting = bettingLimiList,
                PositionTakingSetting = positionTakingList
            };
        }
    }
}