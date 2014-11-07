namespace LicenseeAgent.CommissionSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CommissionSetting.Criteria.MemberList;
    using LicenseeAgent.CommissionSetting.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// MemberCommissionService class.
    /// </summary>
    public class MemberListService : QueryService, IMemberListService
    {
        public IList<MemberCommission> GetMemberCommissions(LicenseeGetMemberCommissionCriteria criteria)
        {
            return GetEntites<MemberCommission>(criteria);
        }

        public IList<MemberCommission> GetMemberCommissions(GetLicenseeMembersByIdsCriteria criteria)
        {
            return GetEntites<MemberCommission>(criteria);
        }

        public void MoveGroup(MemberMoveGroupCommissionCriteria criteria)
        {
            Execute(criteria);
        }

        public void UpdateMember(LicenseeUpdateMemberCommissionCriteria criteria)
        {
            Execute(criteria);
        }

        public void MemberRemoveFromGroup(MemberRemoveGroupCommissionCriteria criteria)
        {
            Execute(criteria);
        }
    }
}