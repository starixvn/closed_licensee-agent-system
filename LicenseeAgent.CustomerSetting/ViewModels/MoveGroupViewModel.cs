namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.Resources;

    /// <summary>
    /// MoveGroupViewModel class.
    /// </summary>
    public class MoveGroupViewModel
    {
        [Required(ErrorMessageResourceName = "GroupNameRequired", ErrorMessageResourceType = typeof(Las))]
        public int GroupCommId { get; set; }

        [Required(ErrorMessageResourceName = "NoSelectedMember", ErrorMessageResourceType = typeof(Las))]
        public string MemberIds { get; set; }
    }
}
