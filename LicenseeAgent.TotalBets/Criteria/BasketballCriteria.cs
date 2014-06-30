namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Basketball Criteria.
    /// </summary>
    public class BasketballCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string LeagueList { get; set; }

        public string MatchList { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_Basketball_MajorBetType";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}