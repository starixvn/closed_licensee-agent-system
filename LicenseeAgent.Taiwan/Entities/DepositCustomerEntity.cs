namespace LicenseeAgent.Taiwan.Entities
{
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Taiwan.Attributes;

    /// <summary>
    /// CustomerEntity class.
    /// </summary>
    public class DepositCustomerEntity
    {
        [Service(Name = "IBCCustId")]
        public int CustId { get; set; }

        public int Currency { get; set; }

        public string UserName { get; set; }

        public bool IsClosed { get; set; }

        [Service(Name = "LASAgentID")]
        public int LASAgentId { get; set; }
    }
}