namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// GroupCommissionService class.
    /// </summary>
    public class GroupCommissionService : QueryService, IGroupCommissionService
    {
        public IEnumerable<GroupCommission> GetGroupCommissions(LicenseeGetGroupCommissionCriteria criteria)
        {
            return GetEntites<GroupCommission>(criteria);
        }
    }
}
