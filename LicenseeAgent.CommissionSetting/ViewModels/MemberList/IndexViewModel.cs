namespace LicenseeAgent.CommissionSetting.ViewModels.MemberList
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CommissionSetting.Entities;
    using LicenseeAgent.Resources;

    /// <summary>
    /// IndexViewModel class.
    /// </summary>
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            GroupCommId = -1;
        }

        [Display(Name = "Username", ResourceType = typeof(Las))]
        public string UserName { get; set; }

        [Display(Name = "Group", ResourceType = typeof(Las))]
        public int GroupCommId { get; set; }

        [Required]
        public int PageSize { get; set; }

        [Required]
        public int PageIndex { get; set; }

        public IList<CommissionGroup> CommissionGroups { get; set; }
    }
}