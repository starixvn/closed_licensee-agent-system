namespace LicenseeAgent.CommissionGroup.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.CommissionGroup.Entities;
    using LicenseeAgent.CommissionGroup.Enumerations;

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
