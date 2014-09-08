namespace LicenseeAgent.Account.Criteria.Account
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentSaltSelectCriteria class.
    /// </summary>
    public class AgentSaltSelectCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public int LicenseeId { get; set; }

        public string UserSalt { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentSaltSel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName);
        }
    }
}