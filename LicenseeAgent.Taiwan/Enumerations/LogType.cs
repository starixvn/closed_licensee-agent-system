namespace LicenseeAgent.Taiwan.Enumerations
{
    using System.Xml.Serialization;

    public enum LogType
    {
        /// <summary>
        /// None status.
        /// </summary>
        [XmlEnum("0")]
        None = 0,

        /// <summary>
        /// Before call API.
        /// </summary>
        [XmlEnum("1")]
        BeforeCall = 1,

        /// <summary>
        /// After call API, response result is successful.
        /// </summary>
        [XmlEnum("2")]
        Success = 2,

        /// <summary>
        /// After call API, response result is failure.
        /// </summary>
        [XmlEnum("3")]
        Fail = 3,

        /// <summary>
        /// Ignore Log.
        /// </summary>
        [XmlEnum("4")]
        Ignore = 4
    }
}