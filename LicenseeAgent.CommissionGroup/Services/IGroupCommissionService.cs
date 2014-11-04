namespace LicenseeAgent.CommissionGroup.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CommissionGroup.Criteria;
    using LicenseeAgent.CommissionGroup.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGroupCommissionService
    {
        IList<GroupCommission> GetGroupCommissions(GroupCommissionSelCriteria criteria);

        GroupCommission GetGroupCommissionById(GroupCommGetByIdCriteria criteria);

        bool GroupCommCheckByName(GroupCommCheckByNameCriteria criteria);

        void GroupCommDel(GroupCommDelCriteria criteria);

        void GroupCommIns(GroupCommInsCriteria criteria);

        void GroupCommUpd(GroupCommUpdCriteria criteria);
    }
}