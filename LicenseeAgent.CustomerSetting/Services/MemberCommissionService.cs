namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// MemberCommissionService class.
    /// </summary>
    public class MemberCommissionService : QueryService, IMemberCommissionService
    {
        public IEnumerable<MemberCommission> GetMemberCommissions(LicenseeGetMemberCommissionCriteria criteria)
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

        public IEnumerable<MemberCommission> GetEffectedMemberCommissions(GetLicenseeMembersByIdsCriteria criteria)
        {
            return GetEntites<MemberCommission>(criteria);
        }
    }
}
