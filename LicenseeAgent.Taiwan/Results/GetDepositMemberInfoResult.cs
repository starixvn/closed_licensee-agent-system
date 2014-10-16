namespace LicenseeAgent.Taiwan.Results
{
    using System.Runtime.Serialization;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// MemberResult class.
    /// </summary>
    [DataContract]
    public class GetDepositMemberInfoResult : ResponseResult
    {
        public GetDepositMemberInfoResult(int errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {
        }

        [DataMember(Name = "Data")]
        public ServiceDepositMemberEntity Member { get; set; }
    }
}