namespace LicenseeAgent.CustomerList.Entities.Member
{
    using System;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Attributes;

    /// <summary>
    /// MemberEntity class.
    /// </summary>
    public class MemberEntity
    {
        [Service(Name = "LASAgentId")]
        public int AgentId { get; set; }

        [Service(Name = "IBCCustId")]
        public int CustId { get; set; }

        public string SystemId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string IpLogin { get; set; }

        public string GroupName { get; set; }

        public int RowNumber { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public int Status { get; set; }

        public decimal AvailableFund { get; set; }

        public decimal PendingFund { get; set; }

        public string CustomerName
        {
            get
            {
                return Extensions.FormatCustomerName(FirstName, LastName);
            }
        }
    }
}