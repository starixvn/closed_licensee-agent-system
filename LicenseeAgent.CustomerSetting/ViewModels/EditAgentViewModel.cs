namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// EditAgentViewModel class.
    /// </summary>
    public class EditAgentViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int AgentId { get; set; }
    }
}