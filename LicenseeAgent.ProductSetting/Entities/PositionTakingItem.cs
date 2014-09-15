namespace LicenseeAgent.ProductSetting.Entities
{
    /// <summary>
    /// PositionTaking class.
    /// </summary>
    public class PositionTakingItem
    {
        public int SportId { get; set; }

        public int BetTypeId { get; set; }

        public bool IsLive { get; set; }

        public decimal Min { get; set; }

        public decimal Max { get; set; }

        public decimal Preset { get; set; }

        public decimal Value { get; set; }

        public bool ApplyAll { get; set; }

        public PositionTakingItem Upline { get; set; }

        public PositionTakingItem AllDownLine { get; set; }
    }
}