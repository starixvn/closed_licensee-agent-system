// ----------------------------------------------------------------------
// <copyright file="ServiceUtils.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Common;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ServiceUtils
    {
        private static JsonSerializerSettings jsonSerializeSettings;

        static ServiceUtils()
        {
            jsonSerializeSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };
        }

        public static JsonSerializerSettings GetJsonSetting
        {
            get
            {
                return jsonSerializeSettings;
            }
        }

        /// <summary>
        /// Build position taking group from raw data.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingData">Position taking raw data.</param>
        /// <param name="maxLicenseePt">Get max PT incase  insert new agent level 1.</param>
        /// <returns>
        /// List position taking group.
        /// </returns>
        public static IList<PositionTakingGroup> BuildPositionTakingGroups(
            int productId,
            SettingContext settingContext,
            Dictionary<string, PositionTaking> positionTakingData,
            int maxLicenseePt = 0)
        {
            var positionTakingSetting = new List<PositionTakingGroup>();
            var productSettings = SettingConfigurations.GetProductPtSettings(productId);
            productSettings.SportType.ForEach(sportType =>
            {
                if (!string.IsNullOrEmpty(sportType.BetTypeIds))
                {
                    var positionTakingGroup = new PositionTakingGroup
                    {
                        SportId = sportType.Id,
                        IsLive = sportType.IsLive,
                        PositionTakings = new List<PositionTaking>()
                    };

                    var betTypeArray = sportType.BetTypeIds.Split(',');
                    foreach (string betTypeId in betTypeArray)
                    {
                        GetPositionTakingData(settingContext, positionTakingData, maxLicenseePt, positionTakingGroup, betTypeId);
                    }

                    positionTakingSetting.Add(positionTakingGroup);
                }
            });

            return positionTakingSetting;
        }

        /// <summary>
        /// Build position taking group from raw data.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="positionTakingData">Position taking raw data.</param>
        /// <returns>
        /// List position taking group.
        /// </returns>
        public static IList<PositionTakingGroup> BuildPositionTakingGroupsForUpdate(
            int productId,
            Dictionary<string, PositionTaking> positionTakingData)
        {
            var positionTakingSetting = new List<PositionTakingGroup>();
            var productSettings = SettingConfigurations.GetProductPtSettings(productId);
            productSettings.SportType.ForEach(sportType =>
            {
                if (!string.IsNullOrEmpty(sportType.BetTypeIds))
                {
                    var positionTakingGroup = new PositionTakingGroup
                    {
                        SportId = sportType.Id,
                        IsLive = sportType.IsLive,
                        PositionTakings = new List<PositionTaking>()
                    };

                    var betTypeArray = sportType.BetTypeIds.Split(',');
                    foreach (string betTypeId in betTypeArray)
                    {
                        GetPositionTakingDataForUpdate(positionTakingData, positionTakingGroup, betTypeId);
                    }

                    positionTakingSetting.Add(positionTakingGroup);
                }
            });

            return positionTakingSetting;
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="commissionSetting">Commission setting data.</param>
        /// <returns>String data for update to database.</returns>
        public static string BuildCommissionSettingXml(SettingContext settingContext, CommissionSetting commissionSetting)
        {
            string xmlStringData = string.Empty;
            if (commissionSetting != null
                && commissionSetting.CommissionItems != null
                && commissionSetting.CommissionItems.Any())
            {
                string tempString;
                if (settingContext.TargetRoleId != (int)Role.Member)
                {
                    foreach (CommissionItem comItem in commissionSetting.CommissionItems)
                    {
                        tempString = string.Format(
                            Constant.FORMAT_AGENT_COMMISSION_SETTING_XML,
                            comItem.SportId,
                            comItem.BetTypeId,
                            comItem.DiscountA / Constant.STEP_MUL_DIV,
                            comItem.DiscountB / Constant.STEP_MUL_DIV,
                            comItem.DiscountC / Constant.STEP_MUL_DIV,
                            comItem.DiscountD / Constant.STEP_MUL_DIV);

                        xmlStringData += tempString;
                    }
                }
                else
                {
                    foreach (CommissionItem comItem in commissionSetting.CommissionItems)
                    {
                        tempString = string.Format(
                            Constant.FORMAT_MEMBER_COMMISSION_SETTING_XML,
                            comItem.SportId,
                            comItem.BetTypeId,
                            comItem.Discount / Constant.STEP_MUL_DIV);

                        xmlStringData += tempString;
                    }
                }
            }

            return xmlStringData;
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="bettingLimits">Betting limits data.</param>
        /// <returns>String data to update database.</returns>
        public static string BuildBetLimitSettingXml(IList<BettingLimit> bettingLimits)
        {
            string xmlStringData = string.Empty;
            if (bettingLimits != null && bettingLimits.Any())
            {
                foreach (BettingLimit betLimit in bettingLimits)
                {
                    xmlStringData += string.Format(
                        Constant.FORMAT_BETTING_LIMIT_XML,
                        betLimit.SportId,
                        betLimit.BetTypeId,
                        betLimit.MinBet,
                        betLimit.MaxBet,
                        betLimit.MaxPerMatch);
                }
            }

            return xmlStringData;
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingSetting">Position taking data.</param>
        /// <returns>String data to update database.</returns>
        public static string BuildPositionTakingSettingXml(SettingContext settingContext, IList<PositionTaking> positionTakingSetting)
        {
            string xmlStringData = string.Empty;

            if (positionTakingSetting == null || !positionTakingSetting.Any())
            {
                return xmlStringData;
            }

            foreach (PositionTaking positionTaking in positionTakingSetting)
            {
                if (positionTaking != null)
                {
                    xmlStringData += BuildXmlForOnePositionTakingSetting(settingContext, positionTaking);
                }
            }

            return xmlStringData;
        }

        /// <summary>
        /// Builds the XML for one position taking setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="positionTaking">The position taking.</param>
        /// <returns>The xml string for one position taking item.</returns>
        public static string BuildXmlForOnePositionTakingSetting(SettingContext settingContext, PositionTaking positionTaking)
        {
            string xmlResult;

            if (settingContext.TargetRoleId != (int)Role.Member)
            {
                if (settingContext.TargetRoleId == (int)Role.Agent && settingContext.FormId == FormType.Edit)
                {
                    xmlResult = string.Format(
                            Constant.FORMAT_AGENT_EDIT_AGENT_POSITION_TAKING_XML,
                            positionTaking.SportId,
                            positionTaking.BetTypeId,
                            positionTaking.IsLive ? 1 : 0,
                            (int)(positionTaking.Preset * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Max * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Min * Constant.STEP_MUL_DIV),
                            positionTaking.ApplyAll ? 1 : 0);
                }
                else
                {
                    xmlResult = string.Format(
                            Constant.FORMAT_AGENT_POSITION_TAKING_XML,
                            positionTaking.SportId,
                            positionTaking.BetTypeId,
                            positionTaking.IsLive ? 1 : 0,
                            (int)(positionTaking.Preset * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Max * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Min * Constant.STEP_MUL_DIV));
                }
            }
            else
            {
                xmlResult = string.Format(
                        Constant.FORMAT_MEMBER_POSITION_TAKING_XML,
                        positionTaking.SportId,
                        positionTaking.BetTypeId,
                        positionTaking.IsLive ? 1 : 0,
                        (int)(positionTaking.Value * Constant.STEP_MUL_DIV));
            }

            return xmlResult;
        }

        /// <summary>
        /// Format JSON return to client.
        /// </summary>
        /// <param name="customerSetting">The number game customer.</param>
        /// <param name="settingContext">Setting Context.</param>
        /// <returns>String JAVASCRIPT.</returns>
        public static StringBuilder FormatJavascript(CustomerSetting customerSetting, SettingContext settingContext = null)
        {
            var result = customerSetting != null ? customerSetting.Result : null;
            var custInfo = customerSetting != null ? customerSetting.CustInfo : null;
            var commissionSetting = customerSetting != null ? customerSetting.CommissionSetting : null;
            var betLimits = customerSetting != null ? customerSetting.BettingLimits : null;
            var positionTakingSetting = customerSetting != null ? customerSetting.PositionTakingSetting : null;

            var settingScript = new StringBuilder();
            if (result != null && result.ErrCode != 0)
            {
                settingScript.Append(string.Format("var result = {0}; \n", JsonConvert.SerializeObject(result, jsonSerializeSettings)));
            }
            else
            {
                if (settingContext != null)
                {
                    settingScript.Append(string.Format("var settingContext= {0}; \n", JsonConvert.SerializeObject(settingContext, jsonSerializeSettings)));
                }

                if (custInfo != null)
                {
                    settingScript.Append(string.Format("var custInfo= {0}; \n", JsonConvert.SerializeObject(custInfo, jsonSerializeSettings)));
                }

                if (commissionSetting != null)
                {
                    settingScript.Append(string.Format("var commissionSetting= {0}; \n", JsonConvert.SerializeObject(commissionSetting, jsonSerializeSettings)));
                }

                if (betLimits != null)
                {
                    settingScript.Append(string.Format("var betLimits= {0}; \n", JsonConvert.SerializeObject(betLimits, jsonSerializeSettings)));
                }

                if (positionTakingSetting != null)
                {
                    settingScript.Append(string.Format("var positionTakingSetting= {0}; \n", JsonConvert.SerializeObject(positionTakingSetting, jsonSerializeSettings)));
                }
            }

            return settingScript;
        }

        /// <summary>
        /// Get Position taking item.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingData">Position taking to save data.</param>
        /// <param name="maxLicenseePt">Max PT in case insert agent level 1.</param>
        /// <param name="positionTakingGroup">Position taking group.</param>
        /// <param name="betTypeId">Bet Type Id.</param>
        private static void GetPositionTakingData(
            SettingContext settingContext,
            Dictionary<string, PositionTaking> positionTakingData,
            int maxLicenseePt,
            PositionTakingGroup positionTakingGroup,
            string betTypeId)
        {
            if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                positionTakingGroup.PositionTakings.Add(new PositionTaking
                {
                    SportId = positionTakingGroup.SportId,
                    IsLive = positionTakingGroup.IsLive,
                    BetTypeId = Converter.ToInt(betTypeId),
                    Upline = new PositionTaking
                    {
                        Max = maxLicenseePt
                    },
                    AllDownLine = new PositionTaking()
                });
            }
            else
            {
                string positionTakingKey = string.Format(
                    "{0}_{1}_{2}",
                    positionTakingGroup.SportId,
                    positionTakingGroup.IsLive,
                    betTypeId);

                if (positionTakingData.ContainsKey(positionTakingKey))
                {
                    positionTakingGroup.PositionTakings.Add(positionTakingData[positionTakingKey]);
                }
            }
        }

        /// <summary>
        /// Get Position taking item.
        /// </summary>
        /// <param name="positionTakingData">Position taking to save data.</param>
        /// <param name="positionTakingGroup">Position taking group.</param>
        /// <param name="betTypeId">Bet Type Id.</param>
        private static void GetPositionTakingDataForUpdate(
            Dictionary<string, PositionTaking> positionTakingData,
            PositionTakingGroup positionTakingGroup,
            string betTypeId)
        {
            string positionTakingKey = string.Format(
                "{0}_{1}_{2}",
                positionTakingGroup.SportId,
                positionTakingGroup.IsLive,
                betTypeId);

            if (positionTakingData.ContainsKey(positionTakingKey))
            {
                positionTakingGroup.PositionTakings.Add(positionTakingData[positionTakingKey]);
            }
        }
    }
}