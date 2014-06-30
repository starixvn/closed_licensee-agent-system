namespace LicenseeAgent.Transfer.ViewModels
{
    using System.Web.Mvc;

    /// <summary>
    /// TransferSearchViewModel class.
    /// </summary>
    public class TransferSearchViewModel
    {
        [AllowHtml]
        public string UserName { get; set; }

        public int? Status { get; set; }
    }
}