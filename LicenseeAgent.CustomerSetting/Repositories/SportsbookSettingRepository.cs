// --------------------------------------------------------------------------
// <copyright file="SportsbookSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Repositories
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.NAP.Framework.Exceptions;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.Sportsbook;

    /// <summary>
    /// Customer repository.
    /// </summary>
    public class SportsbookSettingRepository : ISportsbookSettingRepository
    {
        #region Get Setting

        /// <summary>
        /// Get Agent Position taking for updating.
        /// </summary>
        /// <param name="criteria">Parameters query database.</param>
        /// <returns>Position taking list.</returns>
        public Dictionary<string, PositionTaking> GetAgentPositionTakingSettingForUpdating(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
            }

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                Dictionary<string, PositionTaking> positionTakingDataList = new Dictionary<string, PositionTaking>();
                IDataReader reader = objDb.ExecuteReader(criteria.GetSpParams());
                ReadAgentPositionTakingSettingData(positionTakingDataList, reader);
                return positionTakingDataList;
            }
        }

        /// <summary>
        /// Get Customer position taking for updating.
        /// </summary>
        /// <param name="criteria">Parameters for updating.</param>
        /// <returns>Position taking list.</returns>
        public Dictionary<string, PositionTaking> GetCustomerPositionTakingSettingForUpdating(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
            }

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                Dictionary<string, PositionTaking> positionTakingDataList = new Dictionary<string, PositionTaking>();
                IDataReader reader = objDb.ExecuteReader(criteria.GetSpParams());
                ReadMemberPositionTakingData(positionTakingDataList, reader);

                return positionTakingDataList;
            }
        }

        /// <summary>
        /// Get Bet limit setting for update.
        /// </summary>
        /// <param name="criteria">Criteria for get data from database.</param>
        /// <returns>Betting limit list data.</returns>
        public IList<BettingLimit> GetBettingLimitSettingForUpdating(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
            }

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                IList<BettingLimit> bettingLimits = new List<BettingLimit>();
                IDataReader reader = objDb.ExecuteReader(criteria.GetSpParams());
                ReadBetLimitData(bettingLimits, reader);

                return bettingLimits;
            }
        }

        /// <summary>
        /// Get Agent commission for update.
        /// </summary>
        /// <param name="criteria">Get data criteria.</param>
        /// <returns>List commission item.</returns>
        public IList<CommissionItem> GetAgentCommissionSetingForUpdating(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
            }

            IList<CommissionItem> commissionItems = new List<CommissionItem>();
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                IDataReader reader = objDb.ExecuteReader(criteria.GetSpParams());
                ReadAgentCommissionData(commissionItems, reader);
            }

            return commissionItems;
        }

        /// <summary>
        /// Get Member commission for update.
        /// </summary>
        /// <param name="criteria">Get data commission.</param>
        /// <returns>List commission data.</returns>
        public IList<CommissionItem> GetCustomerCommissionSetingForUpdating(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
            }

            IList<CommissionItem> commissionItems = new List<CommissionItem>();
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                IDataReader reader = objDb.ExecuteReader(criteria.GetSpParams());
                ReadMemberCommissionSettingData(commissionItems, reader);
            }

            return commissionItems;
        }

        /// <summary>
        /// Get Member information.
        /// </summary>
        /// <param name="criteria">Get data criteria.</param>
        /// <returns>Member information details.</returns>
        public MemberInfo GetMemberInfo(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
            }

            using (ObjectDb odb = new ObjectDb(criteria.GetSettingKey()))
            {
                IDataReader reader = odb.ExecuteReader(criteria.GetSpParams());
                reader.Read();
                return new MemberInfo
                {
                    UGroup = reader.ToString("UGroup"),
                    IsUGroupChanged = reader.ToBool("IsUGroupChanged")
                };
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
        /// <param name="msgId">The MSG unique identifier.</param>
        /// <param name="maxLicenseePt">Max PT in case insert agent level 1.</param>
        /// <exception cref="System.Exception">Throw an invalid criteria exception.</exception>
        public void GetSettingForInsert(
            ICriteria criteria,
            SettingContext settingContext,
            out CustomerInfo custInfo,
            out CommissionSetting commissionSetting,
            out IList<BettingLimit> bettingLimits,
            out Dictionary<string, PositionTaking> positionTakingDataList,
            out int msgId,
            out int maxLicenseePt)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
            }

            maxLicenseePt = 0;
            msgId = -1;
            custInfo = new CustomerInfo();
            commissionSetting = new CommissionSetting();
            bettingLimits = new List<BettingLimit>();
            positionTakingDataList = new Dictionary<string, PositionTaking>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    ReadAgentCommissionData(commissionSetting.CommissionItems, reader, false);

                    if (reader.NextResult())
                    {
                        ReadBetLimitData(bettingLimits, reader, false);
                    }

                    if (settingContext.TargetRoleId != (int)Role.AgentLevel1)
                    {
                        if (reader.NextResult())
                        {
                            ReadAgentPositionTakingSettingData(positionTakingDataList, reader, false);
                        }
                    }
                }

                var outputParams = objDb.GetOuputParams();
                msgId = Converter.ToInt(outputParams["msgid"]);
                custInfo.MaxCredit = DbConverter.ToDecimal(outputParams["maxcredit"]);
                if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
                {
                    maxLicenseePt = Converter.ToInt(outputParams["maxptoflicensee"]) / Constant.STEP_MUL_DIV;
                }
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
        /// <param name="msgId">The MSG unique identifier.</param>
        /// <exception cref="System.Exception">Throw an invalid criteria exception.</exception>
        public void GetSettingForUpdate(
            ICriteria criteria,
            SettingContext settingContext,
            out CustomerInfo custInfo,
            out CommissionSetting commissionSetting,
            out IList<BettingLimit> bettingLimits,
            out Dictionary<string, PositionTaking> positionTakingDataList,
            out int msgId)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
            }

            msgId = 0;
            custInfo = new CustomerInfo();
            commissionSetting = new CommissionSetting();
            bettingLimits = new List<BettingLimit>();
            positionTakingDataList = new Dictionary<string, PositionTaking>();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    ParseDataSettingForUpdate(settingContext, custInfo, commissionSetting, bettingLimits, positionTakingDataList, reader);
                }
            }
        }

        #endregion Get Setting

        #region Update Setting

        /// <summary>
        /// Insert agent.
        /// </summary>
        /// <param name="criteria">Insert information.</param>
        /// <returns>Result and output parameters.</returns>
        public InsertResult InsertCustomer(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
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

        /// <summary>
        /// Insert agent.
        /// </summary>
        /// <param name="criteria">Insert information.</param>
        /// <returns>Result and output parameters.</returns>
        public InsertResult UpdateCustomer(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
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

        /// <summary>
        /// Insert agent.
        /// </summary>
        /// <param name="criteria">Insert information.</param>
        /// <returns>Result and output parameters.</returns>
        public InsertResult UpdateIndirectCustomer(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
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

        /// <summary>
        /// Update Com, Bet or PT Setting.
        /// </summary>
        /// <param name="criteria">Criteria for updating data.</param>
        /// <param name="logCriteria">Log criteria.</param>
        /// <returns>Current updating status.</returns>
        public Result UpdateSetting(ICriteria criteria, ICriteria logCriteria)
        {
            if (!criteria.IsValid())
            {
                throw new NapDataAccessException();
            }

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var parameters = logCriteria.AddSpParams(criteria.GetSpParams());
                objDb.ExecuteNonQuery(parameters);
                return new Result(Convert.ToInt32(parameters["errcode"]), Convert.ToString(parameters["errmsg"]));
            }
        }

        #endregion Update Setting

        #region Readers

        /// <summary>
        /// Readers the customer information data.
        /// </summary>
        /// <param name="customerInfo">The customer information.</param>
        /// <param name="reader">The reader.</param>
        private static void ReaderAgentInfoData(CustomerInfo customerInfo, IDataReader reader)
        {
            while (reader.Read())
            {
                customerInfo.UserName = reader.ToString("username");
                customerInfo.FirstName = reader.ToString("firstname");
                customerInfo.LastName = reader.ToString("lastname");
                customerInfo.Status = reader.ToByte("status");
                customerInfo.Credit = reader.ToDecimal("credit");
            }
        }

        /// <summary>
        /// Readers the member information data.
        /// </summary>
        /// <param name="customerInfo">The customer information.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="userGroup">The user group.</param>
        private static void ReaderMemberInfoData(CustomerInfo customerInfo, IDataReader reader, out string userGroup)
        {
            userGroup = string.Empty;
            while (reader.Read())
            {
                customerInfo.UserName = reader.ToString("username");
                customerInfo.FirstName = reader.ToString("firstname");
                customerInfo.LastName = reader.ToString("lastname");
                customerInfo.Status = reader.ToByte("status");
                userGroup = reader.ToString("ugroup");
            }
        }

        /// <summary>
        /// Read member position taking data.
        /// </summary>
        /// <param name="positionTakingDataList">Position taking list after read.</param>
        /// <param name="reader">Reader for reading database.</param>
        private static void ReadMemberPositionTakingData(Dictionary<string, PositionTaking> positionTakingDataList, IDataReader reader)
        {
            while (reader.Read())
            {
                var minPositionTaking = reader.ToDecimal("MinGivenPositionTaking");
                minPositionTaking = minPositionTaking < 0 ? 0 : minPositionTaking;
                var positionTakingData = new PositionTaking
                {
                    SportId = reader.ToInt("SportType"),
                    BetTypeId = reader.ToInt("BetType"),
                    IsLive = reader.ToBool("IsLive"),
                    Value = reader.ToDecimal("PositionTaking") / Constant.STEP_MUL_DIV,
                    Upline = new PositionTaking
                    {
                        Min = minPositionTaking / Constant.STEP_MUL_DIV,
                        Max = reader.ToDecimal("MaxGivenPositionTaking") / Constant.STEP_MUL_DIV
                    }
                };

                positionTakingDataList.Add(
                    string.Format(
                    "{0}_{1}_{2}",
                    positionTakingData.SportId,
                    positionTakingData.IsLive,
                    positionTakingData.BetTypeId),
                    positionTakingData);
            }
        }

        /// <summary>
        /// Read agent position taking setting.
        /// </summary>
        /// <param name="positionTakingDataList">Position taking list after read.</param>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineData">Get up line data or not, in edit form will get this data.</param>
        private static void ReadAgentPositionTakingSettingData(
            Dictionary<string, PositionTaking> positionTakingDataList,
            IDataReader reader,
            bool getUplineData = true)
        {
            while (reader.Read())
            {
                var positionTakingData = new PositionTaking
                {
                    SportId = reader.ToInt("SportType"),
                    BetTypeId = reader.ToInt("BetType"),
                    IsLive = reader.ToBool("IsLive"),
                    Preset = reader.ToDecimal("MaxUpperPositionTaking") / Constant.STEP_MUL_DIV,
                    Max = reader.ToDecimal("MaxGivenPositionTaking") / Constant.STEP_MUL_DIV,
                    Min = reader.ToDecimal("MinGivenPositionTaking") / Constant.STEP_MUL_DIV,
                };

                if (getUplineData)
                {
                    positionTakingData.Upline = new PositionTaking
                    {
                        Min = reader.ToDecimal("MinGivenPositionTakingOfParent") / Constant.STEP_MUL_DIV,
                        Max = reader.ToDecimal("ParentMaxGivenPositionTaking") / Constant.STEP_MUL_DIV
                    };
                    positionTakingData.AllDownLine = new PositionTaking
                    {
                        Max = reader.ToDecimal("MinOfMaxGivenPositionTaking") / Constant.STEP_MUL_DIV
                    };
                }
                else
                {
                    positionTakingData.Upline = new PositionTaking
                    {
                        Min = positionTakingData.Min,
                        Max = positionTakingData.Max
                    };
                    positionTakingData.Preset = positionTakingData.Max;
                    positionTakingData.Value = positionTakingData.Max;
                    positionTakingData.Max = 0;
                    positionTakingData.Min = 0;
                    positionTakingData.AllDownLine = new PositionTaking();
                }

                positionTakingDataList.Add(
                    string.Format(
                    "{0}_{1}_{2}",
                    positionTakingData.SportId,
                    positionTakingData.IsLive,
                    positionTakingData.BetTypeId),
                    positionTakingData);
            }
        }

        /// <summary>
        /// Read agent position taking setting.
        /// </summary>
        /// <param name="positionTakingDataList">Position taking list after read.</param>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="targetRoleId">The target role identifier.</param>
        private static void ReadAgentPositionTakingSettingDataForUpdate(
            Dictionary<string, PositionTaking> positionTakingDataList,
            IDataReader reader,
            int targetRoleId)
        {
            while (reader.Read())
            {
                var positionTakingData = new PositionTaking
                {
                    SportId = reader.ToInt("SportType"),
                    BetTypeId = reader.ToInt("BetType"),
                    IsLive = reader.ToBool("IsLive"),
                    Preset = reader.ToDecimal("MaxUpperPositionTaking") / Constant.STEP_MUL_DIV,
                    Max = reader.ToDecimal("MaxGivenPositionTaking") / Constant.STEP_MUL_DIV,
                    Min = reader.ToDecimal("MinGivenPositionTaking") / Constant.STEP_MUL_DIV,
                };

                if (targetRoleId == (int)Role.Agent)
                {
                    positionTakingData.Upline = new PositionTaking
                    {
                        Max = reader.ToDecimal("ParentMaxGivenPositionTaking") / Constant.STEP_MUL_DIV,
                        Min = reader.ToDecimal("ParentMinGivenPositionTaking") / Constant.STEP_MUL_DIV,
                    };
                }
                else
                {
                    positionTakingData.Upline = new PositionTaking { Max = reader.ToDecimal("ParentMaxGivenPositionTaking") / Constant.STEP_MUL_DIV };
                }

                positionTakingData.AllDownLine = new PositionTaking
                {
                    Max = reader.ToDecimal("MaxOfMaxUpperAndGiven") / Constant.STEP_MUL_DIV
                };

                positionTakingDataList.Add(
                    string.Format(
                    "{0}_{1}_{2}",
                    positionTakingData.SportId,
                    positionTakingData.IsLive,
                    positionTakingData.BetTypeId),
                    positionTakingData);
            }
        }

        /// <summary>
        /// Read member commission setting data.
        /// </summary>
        /// <param name="commissionItems">Commission setting after read.</param>
        /// <param name="reader">Reader for reading data.</param>
        private static void ReadMemberCommissionSettingData(IList<CommissionItem> commissionItems, IDataReader reader)
        {
            while (reader.Read())
            {
                commissionItems.Add(new CommissionItem
                {
                    SportId = reader.ToInt("SportType"),
                    BetTypeId = reader.ToInt("BetType"),
                    Discount = Math.Round((reader.ToDecimal("Discount") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                    Upline = new CommissionItem
                    {
                        DiscountA = Math.Round((reader.ToDecimal("UplineMinDiscountA") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                        DiscountB = Math.Round((reader.ToDecimal("UplineMinDiscountB") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                        DiscountC = Math.Round((reader.ToDecimal("UplineMinDiscountC") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                        DiscountD = Math.Round((reader.ToDecimal("UplineMinDiscountD") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST)
                    }
                });
            }
        }

        /// <summary>
        /// Read agent commission.
        /// </summary>
        /// <param name="commissionItems">Commission setting after read.</param>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineCom">Get up line commission or not.</param>
        private static void ReadAgentCommissionData(IList<CommissionItem> commissionItems, IDataReader reader, bool getUplineCom = true)
        {
            while (reader.Read())
            {
                var commissionItem = new CommissionItem
                {
                    SportId = reader.ToInt("SportType"),
                    BetTypeId = reader.ToInt("BetType"),
                    DiscountA = Math.Round((reader.ToDecimal("DiscountA") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                    DiscountB = Math.Round((reader.ToDecimal("DiscountB") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                    DiscountC = Math.Round((reader.ToDecimal("DiscountC") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                    DiscountD = Math.Round((reader.ToDecimal("DiscountD") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST)
                };

                if (getUplineCom)
                {
                    commissionItem.Upline = new CommissionItem
                    {
                        DiscountA = Math.Round((reader.ToDecimal("UplineMinDiscountA") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                        DiscountB = Math.Round((reader.ToDecimal("UplineMinDiscountB") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                        DiscountC = Math.Round((reader.ToDecimal("UplineMinDiscountC") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST),
                        DiscountD = Math.Round((reader.ToDecimal("UplineMinDiscountD") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST)
                    };
                }
                else
                {
                    commissionItem.Upline = new CommissionItem
                    {
                        DiscountA = commissionItem.DiscountA,
                        DiscountB = commissionItem.DiscountB,
                        DiscountC = commissionItem.DiscountC,
                        DiscountD = commissionItem.DiscountD,
                    };
                }

                commissionItems.Add(commissionItem);
            }
        }

        /// <summary>
        /// Read bet limits setting.
        /// </summary>
        /// <param name="bettingLimits">Bet limits setting after read.</param>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineBetlimit">Get up line bet limits or not.</param>
        private static void ReadBetLimitData(IList<BettingLimit> bettingLimits, IDataReader reader, bool getUplineBetlimit = false)
        {
            while (reader.Read())
            {
                var sportType = reader.ToInt("SportType");
                var bettingLimit = new BettingLimit
                {
                    SportId = sportType,
                    BetTypeId = reader.ToInt("BetType"),
                    MinBet = reader.ToDecimal("MinBet"),
                    MaxBet = reader.ToDecimal("MaxBet"),
                    MaxPerMatch = reader.ToDecimal("MaxPerMatch")
                };

                if (getUplineBetlimit)
                {
                    bettingLimit.MaxMinBet = reader.ToDecimal("MaxMinBet");
                    bettingLimit.MinMaxBet = reader.ToDecimal("MinMaxBet");
                    bettingLimit.MinMaxPerMatch = reader.ToDecimal("MinMaxPerMatch");
                }
                else
                {
                    bettingLimit.MaxMinBet = bettingLimit.MinBet;
                    bettingLimit.MinMaxBet = bettingLimit.MaxBet;
                    bettingLimit.MinMaxPerMatch = bettingLimit.MaxPerMatch;
                }

                bettingLimits.Add(bettingLimit);
            }

            bettingLimits.OrderBy(bt => bt.SportId).OrderBy(bt => bt.BetTypeId).ToList();
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
            CustomerInfo custInfo,
            CommissionSetting commissionSetting,
            IList<BettingLimit> bettingLimits,
            Dictionary<string, PositionTaking> positionTakingDataList,
            IDataReader reader)
        {
            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                var userGroup = string.Empty;
                ReaderMemberInfoData(custInfo, reader, out userGroup);
                commissionSetting.UserGroup = userGroup;
            }
            else
            {
                ReaderAgentInfoData(custInfo, reader);
            }

            if (settingContext.FormId != FormType.EditIndirectMember)
            {
                if (reader.NextResult())
                {
                    if (settingContext.TargetRoleId == (int)Role.Member)
                    {
                        ReadMemberCommissionSettingData(commissionSetting.CommissionItems, reader);
                    }
                    else
                    {
                        ReadAgentCommissionData(commissionSetting.CommissionItems, reader, true);
                    }
                }

                if (reader.NextResult())
                {
                    ReadBetLimitData(bettingLimits, reader, true);
                }
            }

            if (reader.NextResult())
            {
                if (settingContext.TargetRoleId == (int)Role.Member)
                {
                    ReadMemberPositionTakingData(positionTakingDataList, reader);
                }
                else
                {
                    ReadAgentPositionTakingSettingDataForUpdate(positionTakingDataList, reader, settingContext.TargetRoleId);
                }
            }
        }

        #endregion Readers
    }
}