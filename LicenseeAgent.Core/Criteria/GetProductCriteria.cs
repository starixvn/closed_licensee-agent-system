namespace LicenseeAgent.Core.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get List Product Criteria.
    /// </summary>
    public class GetProductCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Licensee_SelProductList";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0;
        }
    }
}