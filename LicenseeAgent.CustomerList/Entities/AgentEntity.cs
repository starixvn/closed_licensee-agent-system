namespace LicenseeAgent.CustomerList.Entities
{
    using System;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// AgentEntity class.
    /// </summary>
    public class AgentEntity
    {
        public AgentEntity()
        {
            AgentLevel = 0;
        }

        public int ParentId { get; set; }

        public string ParentUserName { get; set; }

        public string UserName { get; set; }

        public int AgentId { get; set; }

        public bool HasAgentDownline { get; set; }

        public short AgentLevel { get; set; }

        public int Status { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public DateTime CreatedDate { get; set; }

        public string IpLogin { get; set; }

        public int NumberOfAgent { get; set; }

        public int RowNumber { get; set; }

        public int NumberAgentLevel { get; set; }

        public string CustomerName
        {
            get
            {
                return Extensions.FormatCustomerName(FirstName, LastName);
            }
        }
    }
}