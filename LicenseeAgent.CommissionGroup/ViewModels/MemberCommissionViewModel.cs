namespace LicenseeAgent.CommissionGroup.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.CommissionGroup.Entities;

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

        public IList<GroupCommission> GroupCommissions { get; set; }
    }
}