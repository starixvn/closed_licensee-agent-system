namespace LicenseeAgent.ProductSetting.ViewModels.Status
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// UpdateAgentProductViewModel class.
    /// </summary>
    public class UpdateAgentViewModel
    {
        [Required]
        public int AgentId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public bool Disable { get; set; }

        [Required]
        public bool IsAllDownLine { get; set; }
    }
}