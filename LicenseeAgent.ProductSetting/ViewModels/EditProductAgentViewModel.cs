namespace LicenseeAgent.ProductSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditProductAgentViewModel
    {
        [Required]
        public int AgentId { get; set; }

        [Required]
        public int AgentLevel { get; set; }

        [Required]
        public ProductCode ProductId { get; set; }
    }
}