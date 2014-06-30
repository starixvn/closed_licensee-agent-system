namespace LicenseeAgent.Taiwan.Entities
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// UpdateMemberStatusActionLog class.
    /// </summary>
    [XmlRoot("Root")]
    public class UpdateMemberStatusActionLog
    {
        [XmlElement("Customer")]
        public List<UpdateMemberStatusLogEntity> CustomerLogs { get; set; }
    }
}