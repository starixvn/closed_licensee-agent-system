namespace LicenseeAgent.Account.Criteria.HomePage
{
    using Fanex.Data.Repository;

    /// <summary>
    /// NewMemberOfMonthCriteria class.
    /// </summary>
    public class NewMemberOfMonthCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSelNewMemOfMonth";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}