// -----------------------------------------------------------------------
// <copyright file="IGroupCommissionService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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
        IEnumerable<GroupCommission> GetGroupCommissions(LicenseeGetGroupCommissionCriteria criteria);
    }
}
