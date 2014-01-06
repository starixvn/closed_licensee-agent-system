// --------------------------------------------------------------------------
// <copyright file="ModelSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Configurations
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ModelSettings
    {
        private static List<SelectListItem> _languages;

        private static List<SelectListItem> _betlistStatuses;

        static ModelSettings()
        {
            _languages = new List<SelectListItem>
            {
                new SelectListItem { Text = "English", Value = "en-US" },
                new SelectListItem { Text = "繁體中文", Value = "zh-TW" },
                new SelectListItem { Text = "简体中文", Value = "zh-CN" }
            };

            _betlistStatuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "All", Value = "0" },
                new SelectListItem { Text = "Won", Value = "1" },
                new SelectListItem { Text = "Lose", Value = "2" },
                new SelectListItem { Text = "Draw", Value = "3" }
            };
        }

        public static List<SelectListItem> Languages
        {
            get
            {
                return _languages;
            }
        }

        public static List<SelectListItem> BetListStatuses
        {
            get
            {
                return _betlistStatuses;
            }
        }

        public static List<SelectListItem> StatusList
        {
            get
            {
                var selectItemList = new List<SelectListItem> 
                {
                    new SelectListItem 
                    {
                        Text = Las.All,
                        Value = string.Empty
                    },
                    new SelectListItem 
                    {
                        Text = Las.Closed,
                        Value = ((int)AccountStatus.Closed).ToString()
                    },
                    new SelectListItem
                    {
                        Text = Las.Open,
                        Value = ((int)AccountStatus.Open).ToString()
                    }
                };

                return selectItemList;
            }
        }
    }
}