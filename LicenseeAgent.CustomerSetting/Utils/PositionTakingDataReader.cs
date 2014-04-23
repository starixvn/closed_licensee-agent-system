// --------------------------------------------------------------------------
// <copyright file="PositionTakingDataReader.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Utils
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common.Extensions;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PositionTakingDataReader
    {
        /// <summary>
        /// Read agent position taking setting.
        /// </summary>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="targetRoleId">The target role identifier.</param>
        /// <returns>Returns position taking data.</returns>
        public static Dictionary<string, PositionTaking> ReadAgentPositionTakingDataForUpdate(
            IDataReader reader,
            int targetRoleId)
        {
            var positionTakingDataList = new Dictionary<string, PositionTaking>();
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

                positionTakingDataList.Add(GenerateKey(positionTakingData), positionTakingData);
            }

            return positionTakingDataList;
        }

        /// <summary>
        /// Read agent position taking setting.
        /// </summary>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineData">Get up line data or not, in edit form will get this data.</param>
        /// <returns>Returns data.</returns>
        public static Dictionary<string, PositionTaking> ReadAgentPositionTakingData(
            IDataReader reader,
            bool getUplineData = true)
        {
            var positionTakingDataList = new Dictionary<string, PositionTaking>();
            while (reader.Read())
            {
                var positionTakingData = new PositionTaking
                {
                    SportId = reader.ToInt("SportType"),
                    BetTypeId = reader.ToInt("BetType"),
                    IsLive = reader.ToBool("IsLive"),
                    Preset = reader.ToDecimal("MaxUpperPositionTaking") / Constant.STEP_MUL_DIV,
                    Max = reader.ToDecimal("MaxGivenPositionTaking") / Constant.STEP_MUL_DIV,
                    Min = reader.ToDecimal("MinGivenPositionTaking") / Constant.STEP_MUL_DIV
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

                positionTakingDataList.Add(GenerateKey(positionTakingData), positionTakingData);
            }

            return positionTakingDataList;
        }

        /// <summary>
        /// Read member position taking data.
        /// </summary>
        /// <param name="reader">Reader for reading database.</param>
        /// <returns>Returns position taking data.</returns>
        public static Dictionary<string, PositionTaking> ReadMemberPositionTakingData(IDataReader reader)
        {
            var positionTakingDataList = new Dictionary<string, PositionTaking>();
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

                positionTakingDataList.Add(GenerateKey(positionTakingData), positionTakingData);
            }

            return positionTakingDataList;
        }

        private static string GenerateKey(PositionTaking data)
        {
            return string.Format("{0}_{1}_{2}", data.SportId, data.IsLive, data.BetTypeId);
        }
    }
}