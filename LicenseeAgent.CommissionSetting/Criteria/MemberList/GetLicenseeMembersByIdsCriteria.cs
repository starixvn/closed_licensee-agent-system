namespace LicenseeAgent.CommissionSetting.Criteria.MemberList
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetLicenseeMembersByIdCriteria class.
    /// </summary>
    public class GetLicenseeMembersByIdsCriteria : CriteriaBase
    {
        public string ListCustId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeSelMemberByListId";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ListCustId);
        }
    }
}