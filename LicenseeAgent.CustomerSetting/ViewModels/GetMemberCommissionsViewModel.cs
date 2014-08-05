namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// MemberCommissionViewModel class.
    /// </summary>
    public class GetMemberCommissionsViewModel
    {
        public string UserName { get; set; }

        public int GroupCommId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
