namespace LicenseeAgent.BetList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// MixParlayDetailsCriteria class.
    /// </summary>
    public class MixParlayDetailsCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public long TransId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Betlist_SelParlayDetail";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && TransId > 0;
        }
    }
}