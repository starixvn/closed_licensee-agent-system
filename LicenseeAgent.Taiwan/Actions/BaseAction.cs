namespace LicenseeAgent.Taiwan.Actions
{
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// BaseAction class.
    /// </summary>
    public abstract class BaseAction
    {
        protected BaseAction()
            : this(AppSettings.TaiwanAPI)
        {
        }

        protected BaseAction(string url)
        {
            Url = url;
        }

        [XmlIgnore]
        public string Url { get; private set; }

        public string SiteName { get; set; }

        public abstract string ActionName { get; }

        /// <summary>
        /// Generate Input Parameters Html String.
        /// </summary>
        /// <returns>
        /// Html string.
        /// </returns>
        public string GenerateInputs()
        {
            var inputParamsHtmlString = string.Empty;
            var properties = this.GetType().GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(XmlIgnoreAttribute), true);
                if (property.CanWrite && attribute.Length == 0)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(this, null);

                    inputParamsHtmlString += string.Format("{0}: {1} <br />", propertyName, propertyValue);
                }
            }

            return inputParamsHtmlString;
        }
    }
}