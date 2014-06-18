namespace LicenseeAgent.BetList.Entities
{
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LasTicketData : TicketData
    {
        public decimal AgentPTPercent { get; set; }

        public decimal AgentCommPercent { get; set; }
    }
}