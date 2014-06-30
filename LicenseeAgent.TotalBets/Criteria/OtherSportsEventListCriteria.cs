namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Other Sports Event List Criteria.
    /// </summary>
    public class OtherSportsEventListCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string SportList { get; set; }

        public string LeagueList { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_OtherSports_GetMatchList";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}