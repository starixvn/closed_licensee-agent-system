namespace LicenseeAgent.ProductSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// GetSettingForUpdateViewModel class.
    /// </summary>
    public class GetSettingForUpdateViewModel
    {
        [Required]
        public Product ProductId { get; set; }

        [Required]
        public int CustId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int TargetCustId { get; set; }

        [Required]
        public int TargetRoleId { get; set; }

        [Required]
        public int FormId { get; set; }
    }
}