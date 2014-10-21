namespace LicenseeAgent.ProductSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// InstructionViewModel class.
    /// </summary>
    public class InstructionViewModel
    {
        [Required]
        public string Key { get; set; }

        public string Trigger { get; set; }

        public string Title { get; set; }

        public string TipTitle { get; set; }

        [Required]
        public string Product { get; set; }
    }
}
