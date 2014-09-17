namespace LicenseeAgent.ProductSetting.ViewModels.Status
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// UpdateMemberProductSettingViewModel class.
    /// </summary>
    public class UpdateMemberViewModel
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public bool Disable { get; set; }
    }
}