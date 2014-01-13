// --------------------------------------------------------------------------
// <copyright file="TransferAgentViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferAgentViewModel
    {
        [Required]
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }
    }
}