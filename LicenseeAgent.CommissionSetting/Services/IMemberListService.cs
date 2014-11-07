namespace LicenseeAgent.CommissionSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CommissionSetting.Criteria.MemberList;
    using LicenseeAgent.CommissionSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMemberListService
    {
        IList<MemberCommission> GetMemberCommissions(LicenseeGetMemberCommissionCriteria criteria);

        IList<MemberCommission> GetMemberCommissions(GetLicenseeMembersByIdsCriteria criteria);

        void MoveGroup(MemberMoveGroupCommissionCriteria criteria);

        void UpdateMember(LicenseeUpdateMemberCommissionCriteria criteria);

        void MemberRemoveFromGroup(MemberRemoveGroupCommissionCriteria criteria);
    }
}