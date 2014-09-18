namespace LicenseeAgent.Customer.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentAutoCompleteCriteria class.
    /// </summary>
    public class AgentAutoCompleteCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Text { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetAutoCompleteUserName";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && !string.IsNullOrEmpty(Text);
        }
    }
}