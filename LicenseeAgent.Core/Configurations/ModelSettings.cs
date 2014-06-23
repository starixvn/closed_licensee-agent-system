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
    /// ModelSettings class.
    /// </summary>
    public static class ModelSettings
    {
        private static readonly List<SelectListItem> AllLanguages = new List<SelectListItem>
        {
            new SelectListItem { Text = "English", Value = "en-US" },
            new SelectListItem { Text = "繁體中文", Value = "zh-TW" },
            new SelectListItem { Text = "简体中文", Value = "zh-CN" }
        };

        public static string DefaultLanguage
        {
            get
            {
                return AllLanguages.First().Value;
            }
        }

        public static IList<SelectListItem> Languages
        {
            get
            {
                return AllLanguages;
            }
        }

        public static IList<SelectListItem> BetListStatuses
        {
            get
            {
                return new List<SelectListItem>
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
        }

        public static IList<SelectListItem> TaiwanActions
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = Las.All, Value = "0" },
                    new SelectListItem { Text = "ResetPassword", Value = "301" },
                    new SelectListItem { Text = "UpdateMemberInfo", Value = "303" },
                    new SelectListItem { Text = "InsertNewMember", Value = "304" },
                    new SelectListItem { Text = "UpdateMemberStatus", Value = "305" },
                };
            }
        }

        public static IList<SelectListItem> StatusList
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