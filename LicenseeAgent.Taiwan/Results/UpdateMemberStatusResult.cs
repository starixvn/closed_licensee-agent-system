namespace LicenseeAgent.Taiwan.Results
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// MemberStatusResult class.
    /// </summary>
    [DataContract]
    public class UpdateMemberStatusResult : ResponseResult
    {
        public UpdateMemberStatusResult(int errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {
        }

        [DataMember(Name = "Data")]
        public Collection<MemberStatusEntity> MemberStatuses { get; set; }
    }
}