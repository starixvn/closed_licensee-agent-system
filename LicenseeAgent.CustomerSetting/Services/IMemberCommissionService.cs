namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMemberCommissionService
    {
        IEnumerable<MemberCommission> GetMemberCommissions(LicenseeGetMemberCommissionCriteria criteria);

        void MoveGroup(MemberMoveGroupCommissionCriteria criteria);

        void UpdateMember(LicenseeUpdateMemberCommissionCriteria criteria);

        void MemberRemoveFromGroup(MemberRemoveGroupCommissionCriteria criteria);

        bool IsAllowMemberCommission(AllowedMemberCommissionCriteria criteria);

        IEnumerable<MemberCommission> GetEffectedMemberCommissions(GetLicenseeMembersByIdsCriteria criteria);
    }
}
