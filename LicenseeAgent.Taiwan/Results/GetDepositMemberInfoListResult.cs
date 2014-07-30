namespace LicenseeAgent.Taiwan.Results
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// GetDepositMemberInfoListResult class.
    /// </summary>
    [DataContract]
    public class GetDepositMemberInfoListResult : ResponseResult
    {
        public GetDepositMemberInfoListResult(int errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {
        }

        [DataMember(Name = "Data")]
        public List<DepositMemberInfo> Data { get; set; }
    }
}