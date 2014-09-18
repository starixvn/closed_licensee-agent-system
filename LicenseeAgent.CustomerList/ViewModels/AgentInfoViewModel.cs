namespace LicenseeAgent.Customer.ViewModels
{
    /// <summary>
    /// AgentInfoViewModel class.
    /// </summary>
    public class AgentInfoViewModel
    {
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public short AgentLevel { get; set; }

        public bool IsMember { get; set; }

        public short NumberOfLevel { get; set; }
    }
}