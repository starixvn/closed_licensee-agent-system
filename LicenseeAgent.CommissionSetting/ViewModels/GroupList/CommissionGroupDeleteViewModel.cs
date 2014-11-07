namespace LicenseeAgent.CommissionSetting.ViewModels.GroupList
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CommissionSetting.Enumerations;

    /// <summary>
    /// CommissionGroupDeleteViewModel class.
    /// </summary>
    public class CommissionGroupDeleteViewModel
    {
        [Required]
        public int GroupCommId { get; set; }

        public bool IsDefault { get; set; }

        public DeleteMode Mode { get; set; }
    }
}