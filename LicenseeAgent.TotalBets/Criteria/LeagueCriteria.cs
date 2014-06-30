namespace LicenseeAgent.TotalBets.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Soccer League Criteria.
    /// </summary>
    public class LeagueCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int SportType { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_GetLeagueBySportType";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}