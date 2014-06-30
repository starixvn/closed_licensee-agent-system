namespace LicenseeAgent.Taiwan.Results
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// MemberListResult class.
    /// </summary>
    [DataContract]
    public class MemberListResult : ResponseResult
    {
        public MemberListResult(int errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {
        }

        [DataMember(Name = "Data")]
        public Collection<ServiceMemberEntity> Members { get; set; }
    }
}