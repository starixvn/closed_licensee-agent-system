namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// EditAgentViewModel class.
    /// </summary>
    public class CommissionGroupViewModel
    {
        public string GroupName { get; set; }

        public bool IsDefault { get; set; }

        public List<CommissionProduct> ListCommissonProduct { get; set; }
    }
}