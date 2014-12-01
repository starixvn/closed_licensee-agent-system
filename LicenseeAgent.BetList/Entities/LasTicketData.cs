namespace LicenseeAgent.BetList.Entities
{
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// LasTicketData class.
    /// </summary>
    public class LasTicketData : TicketData
    {
        public decimal AgentPTPercent { get; set; }

        public decimal AgentCommPercent { get; set; }
    }
}