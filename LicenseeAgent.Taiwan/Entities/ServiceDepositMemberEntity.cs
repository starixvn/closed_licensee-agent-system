namespace LicenseeAgent.Taiwan.Entities
{
    using System.Runtime.Serialization;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// ServiceMemberEntity class.
    /// </summary>
    [DataContract]
    public class ServiceDepositMemberEntity
    {
        [DataMember(Name = "IBCCustID")]
        public int IBCCustId { get; set; }

        [DataMember(Name = "Username")]
        public string Username { get; set; }

        [DataMember(Name = "Currency")]
        public int Currency { get; set; }

        [DataMember(Name = "IsClosed")]
        public bool IsClosed { get; set; }

        [DataMember(Name = "LASAgentID")]
        public int LASAgentId { get; set; }

        public int Status
        {
            get
            {
                return IsClosed ? (int)AccountStatus.Closed : (int)AccountStatus.Open;
            }
        }
    }
}