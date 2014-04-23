// ----------------------------------------------------------------------
// <copyright file="NumberGameSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// NumberGameSettingRepository class.
    /// </summary>
    public class NumberGameSettingRepository : INumberGameSettingRepository
    {
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
        public void GetSettingForInsert(
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
                bettingLimits = new List<Entities.NumberGame.BettingLimit>();
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
                    Discount =
                        Math.Round((reader.ToDecimal("Discount") * Constant.STEP_MUL_DIV) - Constant.ROUNDING_NUMBER, Constant.NUMBER_DIGIST)
                };
                commissionItems.Add(commissionItem);
            }
        }

        /// <summary>
        /// Read bet limits setting.
        /// </summary>
        /// <param name="bettingLimits">Bet limits setting after read.</param>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineBetlimit">Get up line bet limits or not.</param>
        private static void ReadBetLimitData(IList<Entities.NumberGame.BettingLimit> bettingLimits, IDataReader reader, bool getUplineBetlimit = false)
        {
            while (reader.Read())
            {
                var sportType = reader.ToInt("SportType");
                var bettingLimit = new Entities.NumberGame.BettingLimit
                {
                    SportId = sportType,
                    BetTypeId = reader.ToInt("BetType"),
                    MinBet = reader.ToDecimal("MinBet"),
                    MaxBet = reader.ToDecimal("MaxBet"),
                    MaxPerMatch = reader.ToDecimal("MaxPerMatch"),
                    MaxPerBall = reader.ToDecimal("MaxPerBall")
                };

                bettingLimits.Add(bettingLimit);
            }

            bettingLimits.OrderBy(bt => bt.SportId).OrderBy(bt => bt.BetTypeId).ToList();
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
    }
}