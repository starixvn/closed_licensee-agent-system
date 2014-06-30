namespace LicenseeAgent.CreditBalance.ViewModels
{
    using System.Web.Mvc;

    /// <summary>
    /// CreditBalanceSearchViewModel class.
    /// </summary>
    public class CreditBalanceSearchViewModel
    {
        [AllowHtml]
        public string UserName { get; set; }

        public int? Status { get; set; }
    }
}