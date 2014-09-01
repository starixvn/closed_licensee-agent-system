namespace LicenseeAgent.Taiwan.Results
{
    using System.Collections.Generic;
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
            Members = new List<ServiceMemberEntity>();
        }

        [DataMember(Name = "Data")]
        public List<ServiceMemberEntity> Members { get; set; }
    }
}