namespace LicenseeAgent.CommissionGroup.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CommissionGroup.Criteria;
    using LicenseeAgent.CommissionGroup.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// GroupCommissionService class.
    /// </summary>
    public class GroupCommissionService : QueryService, IGroupCommissionService
    {
        public IList<GroupCommission> GetGroupCommissions(GroupCommissionSelCriteria criteria)
        {
            return GetEntites<GroupCommission>(criteria);
        }

        public GroupCommission GetGroupCommissionById(GroupCommGetByIdCriteria criteria)
        {
            return GetEntity<GroupCommission>(criteria);
        }

        public bool GroupCommCheckByName(GroupCommCheckByNameCriteria criteria)
        {
            Execute(criteria);
            return criteria.IsAvailable;
        }

        public void GroupCommDel(GroupCommDelCriteria criteria)
        {
            Execute(criteria);
        }

        public void GroupCommIns(GroupCommInsCriteria criteria)
        {
            Execute(criteria);
        }

        public void GroupCommUpd(GroupCommUpdCriteria criteria)
        {
            Execute(criteria);
        }
    }
}