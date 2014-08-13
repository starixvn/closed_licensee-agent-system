namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Enumerations;

    /// <summary>
    /// MemberCommissionViewModel class.
    /// </summary>
    public class CommissionGroupDeleteViewModel
    {
        public int GroupCommId { get; set; }

        public DeleteMode Mode { get; set; }

        public bool IsDefault { get; set; }
    }
}
