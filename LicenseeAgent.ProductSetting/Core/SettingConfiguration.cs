namespace LicenseeAgent.ProductSetting.Core
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

        public static IList<ProductCode> Products3RD
        {
            get
            {
                return new List<ProductCode>
                {
                    ProductCode.Product_1,
                    ProductCode.Product_6,
                    ProductCode.Product_7,
                    ProductCode.Product_4,
                    ProductCode.Product_5,
                    ProductCode.Product_8,
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
            return GetProductPtSettings((int)ProductCode.Product_5).SportType.Select(x => x.Id).ToList();
        }
    }
}