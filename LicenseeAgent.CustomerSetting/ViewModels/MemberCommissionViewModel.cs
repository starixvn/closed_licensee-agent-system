namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// MemberCommissionViewModel class.
    /// </summary>
    public class MemberCommissionViewModel
    {
        public int GroupCommId { get; set; }

        public IEnumerable<GroupCommission> GroupCommissions { get; set; }
    }
}
