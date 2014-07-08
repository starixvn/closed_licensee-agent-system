namespace LicenseeAgent.Taiwan.Entities
{
    using System;
    using System.Runtime.Serialization;
    using LicenseeAgent.Taiwan.Enumerations;

    /// <summary>
    /// MemberStatusEntity class.
    /// </summary>
    [DataContract]
    public class MemberStatusEntity
    {
        [DataMember(Name = "IBCCustID")]
        public int IBCCustId { get; set; }

        [DataMember(Name = "Result")]
        public string Result { get; set; }

        [IgnoreDataMember]
        public LogType Status
        {
            get
            {
                return Result.Equals(ServiceSetting.StatusSuccess, StringComparison.OrdinalIgnoreCase) ? LogType.Success : LogType.Fail;
            }
        }
    }
}