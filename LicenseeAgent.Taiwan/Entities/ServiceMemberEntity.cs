namespace LicenseeAgent.Taiwan.Entities
{
    using System.Runtime.Serialization;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// ServiceMemberEntity class.
    /// </summary>
    [DataContract]
    public class ServiceMemberEntity
    {
        [DataMember(Name = "IBCCustID")]
        public int IBCCustId { get; set; }

        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "LastName")]
        public string LastName { get; set; }

        [DataMember(Name = "Phone")]
        public string Phone { get; set; }

        [DataMember(Name = "MobilePhone")]
        public string MobilePhone { get; set; }

        [DataMember(Name = "Fax")]
        public string Fax { get; set; }

        [DataMember(Name = "IsClosed")]
        public bool IsClosed { get; set; }

        [DataMember(Name = "Username")]
        public string Username { get; set; }

        [DataMember(Name = "FirstDepositDate")]
        public bool FirstDepositDate { get; set; }

        public int Status
        {
            get
            {
                return IsClosed ? (int)AccountStatus.Closed : (int)AccountStatus.Open;
            }
        }
    }
}