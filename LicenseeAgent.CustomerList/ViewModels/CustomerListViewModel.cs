namespace LicenseeAgent.Customer.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// CustomerListViewModel class.
    /// </summary>
    public class CustomerListViewModel
    {
        [Required]
        public int AgentId { get; set; }

        [Required]
        public int AgentLevel { get; set; }

        public int? Status { get; set; }

        [AllowHtml]
        public string UserName { get; set; }

        public bool IsMember { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}