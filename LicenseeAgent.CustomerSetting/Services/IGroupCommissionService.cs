namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;

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