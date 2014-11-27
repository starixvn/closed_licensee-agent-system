namespace LicenseeAgent.BetList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// BetListDetailsCriteria class.
    /// </summary>
    public class TicketDetailsCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public long TransId { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "LAS_Betlist_SelAgentPTComm";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && TransId > 0;
        }
    }
}