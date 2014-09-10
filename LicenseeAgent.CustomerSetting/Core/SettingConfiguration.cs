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
        static SettingConfiguration()
        {
            var configFilePath = HttpContext.Current.Server.MapPath(Constant.PTGridConfig);
            using (var reader = new StreamReader(configFilePath))
            {
                string xml = reader.ReadToEnd();

                // deserialize XML to object
                var xs = new XmlSerializer(typeof(PtSettings));
                PtSettings = (PtSettings)xs.Deserialize(new StringReader(xml));
            }

            Products3RD = new List<Product>
            {
                Product.Product_1,
                Product.Product_6,
                Product.Product_7,
                Product.Product_4,
                Product.Product_5,
                Product.Product_8,
            };

            VirtualSportsTypeIds = GetProductPtSettings((int)Product.Product_5).SportType.Select(x => x.Id).ToList();
        }

        public static IList<Product> Products3RD { get; private set; }

        public static IList<int> VirtualSportsTypeIds { get; private set; }

        public static PtSettings PtSettings { get; private set; }

        public static ProductPtSettings GetProductPtSettings(int productId)
        {
            return PtSettings.Product.Where(x => x.Id == productId).FirstOrDefault();
        }
    }
}