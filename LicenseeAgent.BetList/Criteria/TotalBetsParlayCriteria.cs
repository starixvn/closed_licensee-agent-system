namespace LicenseeAgent.BetList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TotalBetsParlayListCriteria class.
    /// </summary>
    public class TotalBetsParlayCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int BetType { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_SelParlayTickets";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && BetType > 0;
        }
    }
}