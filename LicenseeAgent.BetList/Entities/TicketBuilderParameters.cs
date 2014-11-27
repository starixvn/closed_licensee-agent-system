namespace LicenseeAgent.BetList.Entities
{
    /// <summary>
    /// TicketBuilderParameters class.
    /// </summary>
    public class TicketBuilderParameters
    {
        public BetListResult TicketResult { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsViewPlayerCommission { get; set; }
    }
}