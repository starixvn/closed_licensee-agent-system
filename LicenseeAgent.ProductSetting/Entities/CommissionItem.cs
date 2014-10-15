namespace LicenseeAgent.ProductSetting.Entities
{
    /// <summary>
    /// CommissionItem name.
    /// </summary>
    public class CommissionItem
    {
        public string Group { get; set; }

        public int SportId { get; set; }

        public int BetTypeId { get; set; }

        public decimal Discount { get; set; }

        public decimal DiscountA { get; set; }

        public decimal DiscountB { get; set; }

        public decimal DiscountC { get; set; }

        public decimal DiscountD { get; set; }

        public CommissionItem Upline { get; set; }
    }
}