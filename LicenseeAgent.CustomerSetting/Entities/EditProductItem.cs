namespace LicenseeAgent.CustomerSetting.Entities
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditProductItem
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Verified.")]
        public string Url { get; set; }
    }
}