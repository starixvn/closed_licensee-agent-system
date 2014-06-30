namespace LicenseeAgent.BetList.Entities
{
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// LAS Ticket class.
    /// </summary>
    public class LasTicket : Ticket
    {
        public string ExternalId { get; set; }

        public decimal AgentPTPercent { get; set; }

        public decimal AgentCommPercent { get; set; }

        public string UserName2 { get; set; }
    }
}