namespace LicenseeAgent.ProductSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// GetSettingForInsertViewModel class.
    /// </summary>
    public class GetSettingForInsertViewModel
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int TargetCustId { get; set; }

        [Required]
        public int TargetRoleId { get; set; }
    }
}