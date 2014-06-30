namespace LicenseeAgent.Taiwan.Results
{
    using System.Runtime.Serialization;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// MemberResult class.
    /// </summary>
    [DataContract]
    public class MemberResult : ResponseResult
    {
        public MemberResult(int errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {
        }

        [DataMember(Name = "Data")]
        public ServiceMemberEntity Member { get; set; }
    }
}