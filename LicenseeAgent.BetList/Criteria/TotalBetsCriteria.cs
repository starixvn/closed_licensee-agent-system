namespace LicenseeAgent.BetList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Bet list by match Criteria.
    /// </summary>
    public class TotalBetsCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MatchId { get; set; }

        public int BetType { get; set; }

        public string BetTeam { get; set; }

        public bool? ShowLiveTicket { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Betlist_SelRunningByBettypeMatch";
        }

        public override bool IsValid()
        {
            return (AgentId > 0 && MatchId > 0)
                && (BetType > 0 && !string.IsNullOrEmpty(BetTeam));
        }
    }
}