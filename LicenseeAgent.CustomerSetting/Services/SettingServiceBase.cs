// --------------------------------------------------------------------------
// <copyright file="SettingServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.Common;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class SettingServiceBase
    {
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
        protected IList<PositionTakingGroup> BuildPositionTakingGroups(
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
        protected IList<PositionTakingGroup> BuildPositionTakingGroupsForUpdate(
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
        /// Get Position taking item.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingData">Position taking to save data.</param>
        /// <param name="maxLicenseePt">Max PT in case insert agent level 1.</param>
        /// <param name="positionTakingGroup">Position taking group.</param>
        /// <param name="betTypeId">Bet Type Id.</param>
        private void GetPositionTakingData(
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
        private void GetPositionTakingDataForUpdate(
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