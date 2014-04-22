// --------------------------------------------------------------------------
// <copyright file="SettingConfigurations.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Core
{
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Serialization;

    /// <summary>
    /// Configuration Class, read Xml file and save XDocument object.
    /// </summary>
    public static class SettingConfigurations
    {
        static SettingConfigurations()
        {
            var configFilePath = HttpContext.Current.Server.MapPath(Constant.PT_GRID_CONFIG);
            using (var reader = new StreamReader(configFilePath))
            {
                string xml = reader.ReadToEnd();

                // deserialize XML to object
                var xs = new XmlSerializer(typeof(PtSettings));
                PtSettings = (PtSettings)xs.Deserialize(new StringReader(xml));
            }
        }

        public static PtSettings PtSettings { get; private set; }

        public static ProductPtSettings GetProductPtSettings(int productId)
        {
            return PtSettings.Product.Where(x => x.Id == productId).FirstOrDefault();
        }
    }
}