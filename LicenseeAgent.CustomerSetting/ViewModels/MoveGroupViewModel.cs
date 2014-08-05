namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// MoveGroupViewModel class.
    /// </summary>
    public class MoveGroupViewModel
    {
        public int GroupCommId { get; set; }

        public IEnumerable<GroupCommission> GroupCommission { get; set; }
    }
}
