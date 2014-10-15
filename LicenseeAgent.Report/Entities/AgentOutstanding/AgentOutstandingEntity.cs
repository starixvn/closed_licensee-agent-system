namespace LicenseeAgent.Report.Entities.AgentOutstanding
{
    /// <summary>
    /// Agent Outstanding.
    /// </summary>
    public class AgentOutstandingEntity
    {
        public string Username { get; set; }

        public int Id { get; set; }

        public decimal Outstanding { get; set; }

        public decimal Positiontaking { get; set; }
    }
}