namespace LicenseeAgent.Home.ViewModels.AccountInformation
{
    using System.Collections.Generic;
    using LicenseeAgent.Home.Entities.AccountInformation;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// SearchUserResultViewModel class.
    /// </summary>
    public class SearchUserResultViewModel
    {
        public CustomerEntity Customer { get; set; }

        public IList<UpLineEntity> Uplines { get; set; }

        public IList<DownLineEntity> Downlines { get; set; }

        public IList<ProductSettingEntity> ProductSettings { get; set; }

        public BalanceEntity Balance { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public bool IsShowStatement { get; set; }
    }
}