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
    public static class SettingConfiguration
    {
        private static PtSettings _positionTakingSettings = InitializePtSettings();
        private static IList<int> _virtualSportsTypeIds = InitializeVirtualSportsTypeIds();

        public static IList<Product> Products3RD
        {
            get
            {
                return new List<Product>
                {
                    Product.Product_1,
                    Product.Product_6,
                    Product.Product_7,
                    Product.Product_4,
                    Product.Product_5,
                    Product.Product_8,
                };
            }
        }

        public static IList<int> VirtualSportsTypeIds
        {
            get
            {
                return _virtualSportsTypeIds;
            }
        }

        public static PtSettings PtSettings
        {
            get
            {
                return _positionTakingSettings;
            }
        }

        public static ProductPtSettings GetProductPtSettings(int productId)
        {
            return PtSettings.Product.Where(x => x.Id == productId).FirstOrDefault();
        }

        /// <summary>
        /// Initializes PT settings.
        /// </summary>
        /// <returns>Returns PT settings.</returns>
        private static PtSettings InitializePtSettings()
        {
            var configFilePath = HttpContext.Current.Server.MapPath(Constant.PTGridConfig);
            using (var reader = new StreamReader(configFilePath))
            {
                string xml = reader.ReadToEnd();

                // deserialize XML to object
                var xs = new XmlSerializer(typeof(PtSettings));
                return (PtSettings)xs.Deserialize(new StringReader(xml));
            }
        }

        private static IList<int> InitializeVirtualSportsTypeIds()
        {
            return GetProductPtSettings((int)Product.Product_5).SportType.Select(x => x.Id).ToList();
        }
    }
}