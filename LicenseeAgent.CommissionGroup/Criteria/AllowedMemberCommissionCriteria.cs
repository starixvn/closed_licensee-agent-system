namespace LicenseeAgent.CommissionGroup.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AllowedMemberCommissionCriteria class.
    /// </summary>
    public class AllowedMemberCommissionCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_IsAllowedMemberComm";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0;
        }
    }
}
