namespace LicenseeAgent.Taiwan.Entities
{
    using System.Xml.Serialization;
    using LicenseeAgent.Taiwan.Attributes;
    using LicenseeAgent.Taiwan.Enumerations;

    /// <summary>
    /// UpdateMemberStatusLogEntity class.
    /// </summary>
    public class UpdateMemberStatusLogEntity
    {
        public UpdateMemberStatusLogEntity()
        {
            Status = LogType.BeforeCall;
        }

        [XmlAttribute(AttributeName = "MemberID")]
        [Service(Name = "IBCCustID")]
        public int MemberId { get; set; }

        [XmlAttribute(AttributeName = "AgentID")]
        public int AgentId { get; set; }

        [XmlAttribute(AttributeName = "LogID")]
        public int LogId { get; set; }

        [XmlAttribute(AttributeName = "StatusID")]
        public LogType Status { get; set; }
    }
}