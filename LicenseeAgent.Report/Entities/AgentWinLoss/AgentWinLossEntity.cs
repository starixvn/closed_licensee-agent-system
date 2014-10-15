namespace LicenseeAgent.Report.Entities.AgentWinLoss
{
    /// <summary>
    /// Agent WinLoss.
    /// </summary>
    public class AgentWinLossEntity
    {
        public string Username { get; set; }

        public int AgentId { get; set; }

        public int AgentLevel { get; set; }

        public int CustId { get; set; }

        public long BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winlost { get; set; }

        public decimal PlayerComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal AWinLost { get; set; }

        public decimal AComm { get; set; }

        public decimal ATotal { get; set; }

        public decimal DownlineStake { get; set; }

        public decimal DownlineTotal { get; set; }

        public decimal UplineStake { get; set; }

        public decimal UplineTotal { get; set; }
    }
}