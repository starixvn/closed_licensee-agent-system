// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services.SportsBook
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook.Agent;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook.Licensee;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook.Member;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Services;
    using LicenseeAgent.CustomerSetting.Utils;

    /// <summary>
    /// Customer repository.
    /// </summary>
    public class SettingService : SettingServiceBase, ISettingService
    {
        /// <summary>
        /// Get Agent setting for insert new.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>
        /// Sports book customer setting.
        /// </returns>
        public CustomerSetting2 GetSettingForInsert(SettingContext settingContext, out int errCode)
        {
            CustomerInfo custInfo;
            CommissionSetting commissionSetting;
            IList<BettingLimit> bettingLimits;
            Dictionary<string, PositionTaking> positionTakingData;

            ICriteria getAgentSettingCriteria = GetCriteriaForInsert(settingContext);

            errCode = Result.FailureValue;
            var maxLicenseePt = 0;
            GetSettingForInsert(
                getAgentSettingCriteria,
                settingContext,
                out custInfo,
                out commissionSetting,
                out bettingLimits,
                out positionTakingData,
                out errCode,
                out maxLicenseePt);

            if (errCode == Result.SuccessValue)
            {
                return new CustomerSetting2
                {
                    CustInfo = custInfo,
                    CommissionSetting = commissionSetting,
                    BettingLimits = bettingLimits,
                    PositionTakingSetting = BuildPositionTakingGroups((int)Product.Product_0, settingContext, positionTakingData, maxLicenseePt)
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
            CustomerInfo custInfo;
            CommissionSetting commissionSetting;
            IList<BettingLimit> bettingLimits;
            Dictionary<string, PositionTaking> positionTakingData;
            ICriteria updateCriteria = GetCriteriaForUpdate(settingContext);
            errCode = Result.FailureValue;
            GetSettingForUpdate(
                updateCriteria,
                settingContext,
                out custInfo,
                out commissionSetting,
                out bettingLimits,
                out positionTakingData,
                out errCode);

            if (errCode == Result.SuccessValue)
            {
                return new CustomerSetting2
                {
                    CustInfo = custInfo,
                    CommissionSetting = commissionSetting,
                    BettingLimits = bettingLimits,
                    PositionTakingSetting = BuildPositionTakingGroupsForUpdate((int)Product.Product_0, positionTakingData)
                };
            }

            return null;
        }

        /// <summary>
        /// Insert agent.
        /// </summary>
        /// <param name="criteria">Insert information.</param>
        /// <returns>Result and output parameters.</returns>
        public InsertResult InsertCustomer(ICriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    objDb.ExecuteNonQuery(parameters);
                    return new InsertResult
                    {
                        ErrCode = Converter.ToInt(parameters["msgid"]),
                        MinPTOutput = Converter.To<short>(parameters["minptoutput"]),
                        MaxPTOutput = Converter.To<short>(parameters["maxptoutput"]),
                        MaxCreditOutput = Converter.ToDecimal(parameters["maxcreditoutput"]),
                        BetLimitOutput = Converter.ToDecimal(parameters["betlimitoutput"]),
                        BetLimitSportType = Converter.ToInt(parameters["betlimitsporttype"]),
                        BetLimitBetType = Converter.ToInt(parameters["betlimitbettype"]),
                        PTSportType = Converter.ToInt(parameters["ptsporttype"]),
                        PTBetType = Converter.ToInt(parameters["ptbettype"]),
                        IsLiveOutput = Converter.ToBool(parameters["isliveoutput"]),
                        CustIdReturn = parameters.ContainsKey("agentid") ? Converter.ToInt(parameters["agentid"]) : Converter.ToInt(parameters["custid"]),
                        IBCUserName = parameters.ContainsKey("agentid") ? string.Empty : Converter.ToString2(parameters["ibcusername"])
                    };
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Insert agent.
        /// </summary>
        /// <param name="criteria">Insert information.</param>
        /// <returns>Result and output parameters.</returns>
        public InsertResult UpdateCustomer(ICriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    objDb.ExecuteNonQuery(parameters);
                    return new InsertResult
                    {
                        ErrCode = Converter.ToInt(parameters["msgid"]),
                        MinPTOutput = Converter.To<short>(parameters["minptoutput"]),
                        MaxPTOutput = Converter.To<short>(parameters["maxptoutput"]),
                        BetLimitOutput = Converter.ToDecimal(parameters["betlimitoutput"]),
                        BetLimitSportType = Converter.ToInt(parameters["betlimitsporttype"]),
                        BetLimitBetType = Converter.ToInt(parameters["betlimitbettype"]),
                        PTSportType = Converter.ToInt(parameters["ptsporttype"]),
                        PTBetType = Converter.ToInt(parameters["ptbettype"]),
                        IsLiveOutput = Converter.ToBool(parameters["isliveoutput"]),
                        CustIdReturn = parameters.ContainsKey("agentid") ? Converter.ToInt(parameters["agentid"]) : Converter.ToInt(parameters["custid"])
                    };
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Insert agent.
        /// </summary>
        /// <param name="criteria">Insert information.</param>
        /// <returns>Result and output parameters.</returns>
        public InsertResult UpdateIndirectCustomer(ICriteria criteria)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    objDb.ExecuteNonQuery(parameters);
                    return new InsertResult
                    {
                        ErrCode = Converter.ToInt(parameters["msgid"]),
                        MinPTOutput = Converter.To<short>(parameters["minptoutput"]),
                        MaxPTOutput = Converter.To<short>(parameters["maxptoutput"]),
                        PTSportType = Converter.ToInt(parameters["ptsporttype"]),
                        PTBetType = Converter.ToInt(parameters["ptbettype"]),
                        IsLiveOutput = Converter.ToBool(parameters["isliveoutput"])
                    };
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Parses the data setting for update.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="custInfo">The customer information.</param>
        /// <param name="commissionSetting">The commission setting.</param>
        /// <param name="bettingLimits">The betting limits.</param>
        /// <param name="positionTakingDataList">The position taking data list.</param>
        /// <param name="reader">The reader.</param>
        private static void ParseDataSettingForUpdate(
            SettingContext settingContext,
            out CustomerInfo custInfo,
            out CommissionSetting commissionSetting,
            out IList<BettingLimit> bettingLimits,
            out Dictionary<string, PositionTaking> positionTakingDataList,
            IDataReader reader)
        {
            custInfo = new CustomerInfo();
            commissionSetting = new CommissionSetting();
            positionTakingDataList = new Dictionary<string, PositionTaking>();
            bettingLimits = new List<BettingLimit>();
            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                var userGroup = string.Empty;
                custInfo = CustomerInfoDataReader.ReaderMemberInfo(reader, out userGroup);
                commissionSetting.UserGroup = userGroup;
            }
            else
            {
                custInfo = CustomerInfoDataReader.ReaderAgentInfo(reader);
            }

            if (settingContext.FormId != FormType.EditIndirectMember)
            {
                if (reader.NextResult())
                {
                    if (settingContext.TargetRoleId == (int)Role.Member)
                    {
                        commissionSetting.CommissionItems = CommissionDataReader.ReadMemberCommissionData(reader);
                    }
                    else
                    {
                        commissionSetting.CommissionItems = CommissionDataReader.ReadAgentCommissionData(reader);
                    }
                }

                if (reader.NextResult())
                {
                    bettingLimits = BettingLimitDataReader.ReadBetLimitData(reader, true);
                }
            }

            if (reader.NextResult())
            {
                if (settingContext.TargetRoleId == (int)Role.Member)
                {
                    positionTakingDataList = PositionTakingDataReader.ReadMemberPositionTakingData(reader);
                }
                else
                {
                    positionTakingDataList = PositionTakingDataReader.ReadAgentPositionTakingDataForUpdate(reader, settingContext.TargetRoleId);
                }
            }
        }

        /// <summary>
        /// Get default setting for insert new agent.
        /// </summary>
        /// <param name="criteria">SP parameters.</param>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="custInfo">Customer information.</param>
        /// <param name="commissionSetting">Commission setting.</param>
        /// <param name="bettingLimits">Bet limit setting.</param>
        /// <param name="positionTakingDataList">Position taking setting.</param>
        /// <param name="errCode">The MSG unique identifier.</param>
        /// <param name="maxLicenseePt">Max PT in case insert agent level 1.</param>
        /// <exception cref="System.Exception">Throw an invalid criteria exception.</exception>
        private void GetSettingForInsert(
            ICriteria criteria,
            SettingContext settingContext,
            out CustomerInfo custInfo,
            out CommissionSetting commissionSetting,
            out IList<BettingLimit> bettingLimits,
            out Dictionary<string, PositionTaking> positionTakingDataList,
            out int errCode,
            out int maxLicenseePt)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                maxLicenseePt = 0;
                custInfo = new CustomerInfo();
                commissionSetting = new CommissionSetting();
                bettingLimits = new List<BettingLimit>();
                positionTakingDataList = new Dictionary<string, PositionTaking>();
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        commissionSetting.CommissionItems = CommissionDataReader.ReadAgentCommissionData(reader, false);

                        if (reader.NextResult())
                        {
                            bettingLimits = BettingLimitDataReader.ReadBetLimitData(reader, false);
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
                    errCode = Converter.ToInt(outputParams["msgid"]);
                    custInfo.MaxCredit = DbConverter.ToDecimal(outputParams["maxcredit"]);
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

        /// <summary>
        /// Get default setting for edit customer.
        /// </summary>
        /// <param name="criteria">SP parameters.</param>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="custInfo">Customer information.</param>
        /// <param name="commissionSetting">Commission setting.</param>
        /// <param name="bettingLimits">Bet limit setting.</param>
        /// <param name="positionTakingDataList">Position taking setting.</param>
        /// <param name="errCode">The MSG unique identifier.</param>
        /// <exception cref="System.Exception">Throw an invalid criteria exception.</exception>
        private void GetSettingForUpdate(
            ICriteria criteria,
            SettingContext settingContext,
            out CustomerInfo custInfo,
            out CommissionSetting commissionSetting,
            out IList<BettingLimit> bettingLimits,
            out Dictionary<string, PositionTaking> positionTakingDataList,
            out int errCode)
        {
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                custInfo = new CustomerInfo();
                commissionSetting = new CommissionSetting();
                bettingLimits = new List<BettingLimit>();
                positionTakingDataList = new Dictionary<string, PositionTaking>();

                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        ParseDataSettingForUpdate(
                            settingContext, 
                            out custInfo, 
                            out commissionSetting, 
                            out bettingLimits,
                            out positionTakingDataList, 
                            reader);
                    }

                    var outputParams = objDb.GetOuputParams();
                    errCode = Converter.ToInt(outputParams["msgid"]);
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
            ICriteria getAgentSettingCriteria = null;

            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                if (settingContext.FormId == FormType.EditIndirectMember)
                {
                    getAgentSettingCriteria = new AgentSelectMemberIndirectForUpdateCriteria
                    {
                        AgentId = settingContext.CustId,
                        CustId = settingContext.TargetCustId
                    };
                }
                else
                {
                    getAgentSettingCriteria = new AgentGetMemberSettingForUpdateCriteria
                    {
                        CustId = settingContext.TargetCustId
                    };
                }
            }
            else if (settingContext.TargetRoleId == (int)Role.Agent)
            {
                getAgentSettingCriteria = new AgentGetAgentSettingForUpdateCriteria
                {
                    AgentId = settingContext.TargetCustId
                };
            }
            else if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                getAgentSettingCriteria = new LicenseeGetAgentSettingForUpdateCriteria
                {
                    AgentId = settingContext.TargetCustId,
                };
            }

            return getAgentSettingCriteria;
        }
    }
}