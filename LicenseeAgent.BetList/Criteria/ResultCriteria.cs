namespace LicenseeAgent.BetList.Criteria
{
    /// <summary>
    /// BetListResultCriteria class.
    /// </summary>
    public class ResultCriteria
    {
        public bool IsSportsBook { get; set; }

        public int MatchId { get; set; }

        public long RefNo { get; set; }

        public int SportType { get; set; }
    }
}