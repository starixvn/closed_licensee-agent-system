// --------------------------------------------------------------------------
// <copyright file="CreditBalanceSearchViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

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