namespace LicenseeAgent.Taiwan.Actions
{
    using System;
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Attributes;
    using LicenseeAgent.Taiwan.Criteria;

    /// <summary>
    /// LogAction class.
    /// </summary>
    [Serializable]
    public abstract class LogAction : BaseAction
    {
        [Service]
        [XmlIgnore]
        public int LicenseeId { get; set; }

        [Service]
        [XmlIgnore]
        public int UserId { get; set; }

        public abstract LogActionCriteria CreateCriteria();

        protected string GetDoingLogXml()
        {
            return ObjectSerializer<LogAction>.Serialize(this);
        }
    }
}