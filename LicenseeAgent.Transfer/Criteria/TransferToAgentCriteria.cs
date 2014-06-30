namespace LicenseeAgent.Transfer.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TransferToAgentCriteria class.
    /// </summary>
    public class TransferToAgentCriteria : CriteriaBase
    {
        public int Updater { get; set; }

        public int AgentId { get; set; }

        public int ToAgentId { get; set; }

        public string IP { get; set; }

        public decimal Amount { get; set; }

        public decimal YesterdayBalance { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Transfer_AgentToAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && ToAgentId > 0;
        }
    }
}