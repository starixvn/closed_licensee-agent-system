// --------------------------------------------------------------------------
// <copyright file="SettingServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System;
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
        /// <param name="positionTakingList">Position taking raw data.</param>
        /// <param name="maxLicenseePt">Get max PT incase  insert new agent level 1.</param>
        /// <returns>
        /// List position taking group.
        /// </returns>
        protected IList<PositionTakingGroup> BuildPositionTakingGroups(
            int productId,
            SettingContext settingContext,
            IList<PositionTakingItem> positionTakingList,
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
                        PositionTakings = new List<PositionTakingItem>()
                    };

                    var betTypeArray = sportType.BetTypeIds.Split(',');
                    foreach (string betTypeId in betTypeArray)
                    {
                        GetPositionTakingData(settingContext, positionTakingList, maxLicenseePt, positionTakingGroup, Convert.ToInt32(betTypeId));
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
        /// <param name="positionTakingList">Position taking raw data.</param>
        /// <returns>
        /// List position taking group.
        /// </returns>
        protected IList<PositionTakingGroup> BuildPositionTakingGroupsForUpdate(
            int productId,
            IList<PositionTakingItem> positionTakingList)
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
                        PositionTakings = new List<PositionTakingItem>()
                    };

                    var betTypeArray = sportType.BetTypeIds.Split(',');
                    foreach (string betTypeId in betTypeArray)
                    {
                        GetPositionTakingDataForUpdate(positionTakingList, positionTakingGroup, Convert.ToInt32(betTypeId));
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
        /// <param name="positionTakingList">Position taking to save data.</param>
        /// <param name="maxLicenseePt">Max PT in case insert agent level 1.</param>
        /// <param name="positionTakingGroup">Position taking group.</param>
        /// <param name="betTypeId">Bet Type Id.</param>
        private void GetPositionTakingData(
            SettingContext settingContext,
            IList<PositionTakingItem> positionTakingList,
            int maxLicenseePt,
            PositionTakingGroup positionTakingGroup,
            int betTypeId)
        {
            if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                positionTakingGroup.PositionTakings.Add(new PositionTakingItem
                {
                    SportId = positionTakingGroup.SportId,
                    IsLive = positionTakingGroup.IsLive,
                    BetTypeId = Converter.ToInt(betTypeId),
                    Upline = new PositionTakingItem
                    {
                        Max = maxLicenseePt
                    },
                    AllDownLine = new PositionTakingItem()
                });
            }
            else
            {
                string positionTakingKey = positionTakingGroup.GenerateKey(betTypeId);
                foreach (var positionTakingItem in positionTakingList)
                {
                    if (positionTakingItem.GenerateKey() == positionTakingKey)
                    {
                        positionTakingGroup.PositionTakings.Add(positionTakingItem);
                    }
                }
            }
        }

        /// <summary>
        /// Get Position taking item.
        /// </summary>
        /// <param name="positionTakingList">Position taking to save data.</param>
        /// <param name="positionTakingGroup">Position taking group.</param>
        /// <param name="betTypeId">Bet Type Id.</param>
        private void GetPositionTakingDataForUpdate(
            IList<PositionTakingItem> positionTakingList,
            PositionTakingGroup positionTakingGroup,
            int betTypeId)
        {
            var positionTakingKey = positionTakingGroup.GenerateKey(betTypeId);
            foreach (var positionTakingItem in positionTakingList)
            {
                if (positionTakingItem.GenerateKey() == positionTakingKey)
                {
                    positionTakingGroup.PositionTakings.Add(positionTakingItem);
                }
            }
        }
    }
}