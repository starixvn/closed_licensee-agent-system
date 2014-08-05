namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// MoveGroupViewModel class.
    /// </summary>
    public class UpdateMoveGroupViewModel
    {
        [Required]
        public int GroupCommId { get; set; }

        [Required]
        public string MemberIds { get; set; }
    }
}
