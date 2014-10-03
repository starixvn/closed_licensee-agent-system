namespace LicenseeAgent.Taiwan.Entities
{
    using System.Runtime.Serialization;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// DepositMemberInfo class.
    /// </summary>
    [DataContract]
    public class DepositMemberInfo
    {
        [DataMember(Name = "IBCCustID")]
        public int IBCCustId { get; set; }

        [DataMember(Name = "Username")]
        public string Username { get; set; }

        [DataMember(Name = "IsClosed")]
        public bool IsClosed { get; set; }

        public int Status
        {
            get
            {
                return IsClosed ? (int)AccountStatus.Closed : (int)AccountStatus.Open;
            }
        }
    }
}