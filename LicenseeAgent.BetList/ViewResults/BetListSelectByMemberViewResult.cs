namespace LicenseeAgent.BetList.ViewResults
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// BetListSelectByMemberViewResult class.
    /// </summary>
    public class BetListSelectByMemberViewResult
    {
        public int CustId { get; set; }

        public IList<ProductInfo> ProductList { get; set; }

        public bool IsAdmin { get; set; }
    }
}