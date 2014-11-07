namespace LicenseeAgent.CommissionSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CommissionSetting.Criteria;
    using LicenseeAgent.CommissionSetting.Criteria.GroupList;
    using LicenseeAgent.CommissionSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGroupListService
    {
        IList<CommissionGroup> GetCommissionGroups(GroupCommissionSelCriteria criteria);

        CommissionGroup GetCommissionGroup(GroupCommGetByIdCriteria criteria);

        bool CheckGroupAvailability(GroupCommCheckByNameCriteria criteria);

        void DeleteGroup(GroupCommDelCriteria criteria);

        void InsertGroup(GroupCommInsCriteria criteria);

        void UpdateGroup(GroupCommUpdCriteria criteria);
    }
}