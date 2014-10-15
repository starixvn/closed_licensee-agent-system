namespace LicenseeAgent.CustomerList.Criteria.Member
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Up line By Customer Id Criteria.
    /// </summary>
    public class GetUplineByCustIdCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetUplineByCustId";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}