namespace LicenseeAgent.Account.Criteria.HomePage
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMemberSiteLinkCriteria class.
    /// </summary>
    public class GetMemberSiteLinkCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        [SpParam(Ignored = true)]
        public string AgentUserName { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_HomePage_SelMemberSiteLink";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0;
        }
    }
}