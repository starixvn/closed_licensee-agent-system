namespace LicenseeAgent.TotalBets.Core
{
    using System.IO;
    using System.Web;
    using System.Xml.Serialization;

    /// <summary>
    /// Configuration Class, read Xml file and save XDocument object.
    /// </summary>
    public static class SettingConfigurations
    {
        static SettingConfigurations()
        {
            var configFilePath = HttpContext.Current.Server.MapPath(Constant.SoccerDetailConfigFile);
            using (var reader = new StreamReader(configFilePath))
            {
                string xml = reader.ReadToEnd();

                // deserialize XML to object
                var xs = new XmlSerializer(typeof(SoccerDetailConfiguration));
                SoccerDetailConfiguration = (SoccerDetailConfiguration)xs.Deserialize(new StringReader(xml));
            }
        }

        public static SoccerDetailConfiguration SoccerDetailConfiguration { get; private set; }
    }
}