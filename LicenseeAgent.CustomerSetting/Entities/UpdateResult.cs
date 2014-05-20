namespace LicenseeAgent.CustomerSetting.Entities
{
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.CustomerSetting.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [XmlType("SettingOutput")]
    public class UpdateResult : Result
    {
        public ErrorType ErrType { get; set; }

        public int BetType { get; set; }

        public int SportType { get; set; }

        public decimal MaxCredit { get; set; }

        public decimal BetLimit { get; set; }

        public decimal MinPT { get; set; }

        public decimal MaxPT { get; set; }

        [XmlElement(ElementName = "IsLive")]
        public int BetStatus { get; set; }

        [XmlIgnore]
        public bool IsLive 
        {
            get
            {
                return BetStatus == 1;
            }
        }
    }
}
