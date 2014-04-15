// --------------------------------------------------------------------------
// <copyright file="SettingConfigurations.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Core
{
    using System.Web;
    using System.Xml.Linq;

    /// <summary>
    /// Configuration Class, read Xml file and save XDocument object.
    /// </summary>
    public static class SettingConfigurations
    {
        static SettingConfigurations()
        {
            PositionTakingGridConfig = XDocument.Load(HttpContext.Current.Server.MapPath(Constant.PT_GRID_CONFIG));
        }

        public static XDocument PositionTakingGridConfig { get; set; }

        public static XDocument BettingLimitSportNameConfig { get; set; }
    }
}