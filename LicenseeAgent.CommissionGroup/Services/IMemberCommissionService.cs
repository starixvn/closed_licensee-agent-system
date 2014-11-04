namespace LicenseeAgent.CommissionGroup.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CommissionGroup.Criteria;
    using LicenseeAgent.CommissionGroup.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMemberCommissionService
    {
        IEnumerable<MemberCommission> GetMemberCommissions(LicenseeGetMemberCommissionCriteria criteria);

        void MoveGroup(MemberMoveGroupCommissionCriteria criteria);

        void UpdateMember(LicenseeUpdateMemberCommissionCriteria criteria);

        void MemberRemoveFromGroup(MemberRemoveGroupCommissionCriteria criteria);

        IEnumerable<MemberCommission> GetEffectedMemberCommissions(GetLicenseeMembersByIdsCriteria criteria);
    }
}
