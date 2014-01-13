// --------------------------------------------------------------------------
// <copyright file="TransferMemberViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferMemberViewModel
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}