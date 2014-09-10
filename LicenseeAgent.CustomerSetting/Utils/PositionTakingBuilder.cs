namespace LicenseeAgent.CustomerSetting.Utils
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class PositionTakingBuilder
    {
        public static string GenerateKey(int sportId, bool isLive, int betTypeId)
        {
            return string.Format("{0}_{1}_{2}", sportId, isLive, betTypeId);
        }

        /// <summary>
        /// Build position taking group from raw data.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingList">Position taking raw data.</param>
        /// <param name="maxLicenseePT">Get max PT incase  insert new agent level 1.</param>
        /// <returns>
        /// List position taking group.
        /// </returns>
        public static IList<PositionTakingGroup> BuildGroups(
            int productId,
            SettingContext settingContext,
            IList<PositionTakingItem> positionTakingList,
            int maxLicenseePT = 0)
        {
            var positionTakingSetting = new List<PositionTakingGroup>();
            var productSettings = SettingConfiguration.GetProductPtSettings(productId);
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
                        BuildGroup(settingContext, positionTakingList, maxLicenseePT, positionTakingGroup, Convert.ToInt32(betTypeId));
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
        public static IList<PositionTakingGroup> BuildGroups(
            int productId,
            IList<PositionTakingItem> positionTakingList)
        {
            var positionTakingSetting = new List<PositionTakingGroup>();
            var productSettings = SettingConfiguration.GetProductPtSettings(productId);
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
                        BuildGroup(positionTakingList, positionTakingGroup, Convert.ToInt32(betTypeId));
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
        private static void BuildGroup(
            SettingContext settingContext,
            IList<PositionTakingItem> positionTakingList,
            int maxLicenseePt,
            PositionTakingGroup positionTakingGroup,
            int betTypeId)
        {
            if (settingContext.FormId == FormType.Add && settingContext.TargetRoleId == (int)Role.AgentLevel1)
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
                string positionTakingKey = PositionTakingBuilder.GenerateKey(positionTakingGroup.SportId, positionTakingGroup.IsLive, betTypeId);
                foreach (var positionTakingItem in positionTakingList)
                {
                    if (PositionTakingBuilder.GenerateKey(positionTakingItem.SportId, positionTakingItem.IsLive, positionTakingItem.BetTypeId) == positionTakingKey)
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
        private static void BuildGroup(
            IList<PositionTakingItem> positionTakingList,
            PositionTakingGroup positionTakingGroup,
            int betTypeId)
        {
            var positionTakingKey = PositionTakingBuilder.GenerateKey(positionTakingGroup.SportId, positionTakingGroup.IsLive, betTypeId);
            foreach (var positionTakingItem in positionTakingList)
            {
                if (PositionTakingBuilder.GenerateKey(positionTakingItem.SportId, positionTakingItem.IsLive, positionTakingItem.BetTypeId) == positionTakingKey)
                {
                    positionTakingGroup.PositionTakings.Add(positionTakingItem);
                }
            }
        }
    }
}