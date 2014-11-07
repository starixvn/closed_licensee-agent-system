namespace LicenseeAgent.CommissionSetting.ViewModels.GroupList
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// CommissionGroupAddViewModell class.
    /// </summary>
    public class CommissionGroupEditViewModel : CommissionGroupAddViewModel
    {
        [Required]
        public int GroupCommId { get; set; }

        public int? DefaultGroupId { get; set; }

        public bool IsResetDefault
        {
            get
            {
                return IsDefault && DefaultGroupId.HasValue && GroupCommId != DefaultGroupId.Value;
            }
        }
    }
}