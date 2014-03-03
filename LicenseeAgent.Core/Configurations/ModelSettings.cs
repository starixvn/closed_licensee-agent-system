// --------------------------------------------------------------------------
// <copyright file="ModelSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Configurations
{
    using System.Collections.Generic;
    using System.Linq;
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
                new SelectListItem { Text = Las.All, Value = "255" },
                new SelectListItem { Text = Las.Waiting, Value = "1" },
                new SelectListItem { Text = Las.Running, Value = "0" },
                new SelectListItem { Text = Las.Won, Value = "112" },
                new SelectListItem { Text = Las.Lose, Value = "113" },
                new SelectListItem { Text = Las.Draw, Value = "111" },
                new SelectListItem { Text = Las.Void, Value = "102" },
                new SelectListItem { Text = Las.Reject, Value = "101" },
                new SelectListItem { Text = Las.Refund, Value = "103" },
            };
        }

        public static string DefaultLanguage
        {
            get
            {
                return _languages.First().Value;
            }
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