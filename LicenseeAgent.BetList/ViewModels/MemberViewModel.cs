namespace LicenseeAgent.BetList.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// BetListSelectByMemberViewModel class.
    /// </summary>
    public class MemberViewModel
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public string Status { get; set; }

        public string Product { get; set; }

        public bool IsAdmin { get; set; }

        public IList<ProductInfo> ProductList { get; set; }
    }
}