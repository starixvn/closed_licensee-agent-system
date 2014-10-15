namespace LicenseeAgent.ProductSetting.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.ProductSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditProductViewModel
    {
        public int ProductId { get; set; }

        public List<EditProductItem> Products { get; set; }
    }
}