namespace LicenseeAgent.ProductSetting.Entities
{
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// Result of insert command.
    /// </summary>
    public class InsertResult : Result, ISettingResult
    {
        public int CustIdReturn { get; set; }

        public string IBCUserName { get; set; }

        public decimal MinPT { get; set; }

        public decimal MaxPT { get; set; }

        public decimal MaxCredit { get; set; }

        public decimal BetLimit { get; set; }

        public bool IsLive { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }
    }
}