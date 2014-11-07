namespace LicenseeAgent.MemberArrangement.Entities
{
    using System;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// Agent information.
    /// </summary>
    [Serializable]
    public class AgentInformation
    {
        public int AgentId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Currency { get; set; }

        public string AgentLevel { get; set; }

        public short LegendType { get; set; }

        public short Status { get; set; }

        public bool IsClosed
        {
            get
            {
                return Status == 0;
            }
        }
    }
}