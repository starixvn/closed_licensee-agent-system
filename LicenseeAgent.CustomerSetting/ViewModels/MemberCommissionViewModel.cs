namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// MemberCommissionViewModel class.
    /// </summary>
    public class MemberCommissionViewModel
    {
        public int GroupCommId { get; set; }

        public bool HasSportbook { get; set; }

        public bool HasRacing { get; set; }

        public bool HasNumberGame { get; set; }

        public bool HasLiveCasino { get; set; }

        public bool HasRNGCasino { get; set; }

        public int ProductCount { get; set; }

        public IEnumerable<GroupCommission> GroupCommissions { get; set; }
    }
}
