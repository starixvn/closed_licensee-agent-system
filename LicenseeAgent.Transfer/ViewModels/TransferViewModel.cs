namespace LicenseeAgent.Transfer.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TransferSearchViewModel class.
    /// </summary>
    public class TransferViewModel
    {
        [AllowHtml]
        [Display(Name = "Username", ResourceType = typeof(Las))]
        public string UserName { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Las))]
        public int? Status { get; set; }
    }
}