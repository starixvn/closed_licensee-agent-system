namespace LicenseeAgent.CommissionSetting.Entities
{
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// ProductCommission class.
    /// </summary>
    public class ProductCommissionColumn
    {
        public ProductCommissionColumn(ProductCode productId, string headerResource, string columnName)
        {
            ProductId = (int)productId;
            HeaderResource = headerResource;
            ColumnName = columnName;
        }

        public int ProductId { get; private set; }

        public string HeaderResource { get; private set; }

        public string ColumnName { get; private set; }
    }
}