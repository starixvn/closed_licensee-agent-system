namespace LicenseeAgent.CommissionSetting.ViewModels.MemberList
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CommissionSetting.Entities;
    using LicenseeAgent.Resources;

    /// <summary>
    /// MoveGroupViewModel class.
    /// </summary>
    public class MoveToGroupViewModel
    {
        [Required(ErrorMessageResourceName = "GroupNameRequired", ErrorMessageResourceType = typeof(Las))]
        [Display(Name = "SelectGroup", ResourceType = typeof(Las))]
        public int GroupCommId { get; set; }

        [Required(ErrorMessageResourceName = "NoSelectedMember", ErrorMessageResourceType = typeof(Las))]
        public string CustIds { get; set; }

        public IList<CommissionGroup> CommissionGroups { get; set; }

        public IList<ProductCommissionColumn> ProductColumns { get; set; }
    }
}