namespace LicenseeAgent.CommissionSetting.Entities
{
    /// <summary>
    /// ProductGroupCommission class.
    /// </summary>
    public class ProductCommission
    {
        public int ProductId { get; set; }

        public decimal Commission { get; set; }

        public string HeaderResource { get; set; }
    }
}