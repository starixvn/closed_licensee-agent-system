namespace LicenseeAgent.CreditBalance.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;

    /// <summary>
    /// CreditBalanceSearchViewModel class.
    /// </summary>
    public class CreditBalanceViewModel
    {
        [AllowHtml]
        [Display(Name = "Username", ResourceType = typeof(Las))]
        public string UserName { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Las))]
        public int? Status { get; set; }
    }
}