namespace LicenseeAgent.CommissionSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CommissionSetting.Criteria;
    using LicenseeAgent.CommissionSetting.Criteria.GroupList;
    using LicenseeAgent.CommissionSetting.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// GroupCommissionService class.
    /// </summary>
    public class GroupListService : QueryService, IGroupListService
    {
        public IList<CommissionGroup> GetCommissionGroups(GroupCommissionSelCriteria criteria)
        {
            return GetEntites<CommissionGroup>(criteria);
        }

        public CommissionGroup GetCommissionGroup(GroupCommGetByIdCriteria criteria)
        {
            return GetEntity<CommissionGroup>(criteria);
        }

        public bool CheckGroupAvailability(GroupCommCheckByNameCriteria criteria)
        {
            Execute(criteria);
            return criteria.IsAvailable;
        }

        public void DeleteGroup(GroupCommDelCriteria criteria)
        {
            Execute(criteria);
        }

        public void InsertGroup(GroupCommInsCriteria criteria)
        {
            Execute(criteria);
        }

        public void UpdateGroup(GroupCommUpdCriteria criteria)
        {
            Execute(criteria);
        }
    }
}