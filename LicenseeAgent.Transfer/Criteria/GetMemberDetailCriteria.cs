namespace LicenseeAgent.Transfer.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMemberDetailCriteria class.
    /// </summary>
    public class GetMemberDetailCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Transfer_MemberDetail";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}