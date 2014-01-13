// --------------------------------------------------------------------------
// <copyright file="TransferMemberViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferMemberViewModel
    {
        [Required]
        public int MemberId { get; set; }

        public string UserName { get; set; }

        public decimal BetCredit { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime LastTransaction { get; set; }

        [Required]
        [Display(Name = "Amount", ResourceType = typeof(Las))]
        public decimal Amount { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }
    }
}