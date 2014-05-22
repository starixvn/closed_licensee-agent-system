// -----------------------------------------------------------------------
// <copyright file="EditProductViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditProductViewModel
    {
        public Product ProductId { get; set; }

        public List<EditProductItem> Products { get; set; }
    }
}