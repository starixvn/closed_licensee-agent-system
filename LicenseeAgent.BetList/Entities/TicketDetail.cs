namespace LicenseeAgent.BetList.Entities
{
    /// <summary>
    /// TicketDetail class.
    /// </summary>
    public class TicketDetail
    {
        public int AgentId { get; set; }

        public int AgentLevel { get; set; }

        public string AgentName { get; set; }

        public decimal AgentPT { get; set; }

        public decimal AgentPTPercent { get; set; }

        public decimal AgentComm { get; set; }

        public decimal AgentCommPercent { get; set; }
    }
}