// -----------------------------------------------------------------------
// <copyright file="CustomerListViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
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

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}