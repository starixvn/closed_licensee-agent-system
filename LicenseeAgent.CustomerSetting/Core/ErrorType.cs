namespace LicenseeAgent.CustomerSetting.Core
{
    using System.Xml.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum ErrorType : int
    {
        /// <summary>
        /// Default value.
        /// </summary>
        [XmlEnum("0")]
        Default = 0,

        /// <summary>
        /// Commission Type.
        /// </summary>
        [XmlEnum("1")]
        Commission = 1,

        /// <summary>
        /// Bet Setting Type.
        /// </summary>
        [XmlEnum("2")]
        BetSetting = 2,

        /// <summary>
        /// Position Taking Type.
        /// </summary>
        [XmlEnum("3")]
        PositionTaking = 3,

        /// <summary>
        /// Max Credit Type.
        /// </summary>
        [XmlEnum("4")]
        MaxCredit = 4
    }
}