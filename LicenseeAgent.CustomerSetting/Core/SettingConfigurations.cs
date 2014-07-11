namespace LicenseeAgent.CustomerSetting.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// Configuration Class, read Xml file and save XDocument object.
    /// </summary>
    public static class SettingConfigurations
    {
        static SettingConfigurations()
        {
            var configFilePath = HttpContext.Current.Server.MapPath(Constant.PTGridConfig);
            using (var reader = new StreamReader(configFilePath))
            {
                string xml = reader.ReadToEnd();

                // deserialize XML to object
                var xs = new XmlSerializer(typeof(PtSettings));
                PtSettings = (PtSettings)xs.Deserialize(new StringReader(xml));
            }
        }

        public static PtSettings PtSettings { get; private set; }

        public static List<int> VirtualSportsTypeIds
        {
            get
            {
                return GetProductPtSettings((int)Product.Product_5).SportType.Select(x => x.Id).ToList();
            }
        }

        public static ProductPtSettings GetProductPtSettings(int productId)
        {
            return PtSettings.Product.Where(x => x.Id == productId).FirstOrDefault();
        }
    }
}