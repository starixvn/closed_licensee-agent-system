namespace LicenseeAgent.ProductSetting.Entities
{
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.ProductSetting.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [XmlType("SettingOutput")]
    public class UpdateResult : Result, ISettingResult
    {
        public ErrorType ErrType { get; set; }

        public int BetType { get; set; }

        public int SportType { get; set; }

        public decimal MaxCredit { get; set; }

        public decimal BetLimit { get; set; }

        public decimal MinPT { get; set; }

        public decimal MaxPT { get; set; }

        [XmlElement(ElementName = "IsLive")]
        public int IsLiveInternal { get; set; }

        [XmlIgnore]
        public bool IsLive
        {
            get
            {
                return IsLiveInternal == 1;
            }

            set
            {
                IsLiveInternal = value ? 1 : 0;
            }
        }
    }
}