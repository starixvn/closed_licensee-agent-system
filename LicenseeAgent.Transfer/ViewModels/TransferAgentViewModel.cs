// --------------------------------------------------------------------------
// <copyright file="TransferAgentViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    public class TransferAgentViewModel
    {
        [Required]
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        [Required]
        [Display(Name = "Amount", ResourceType = typeof(Las))]
        public decimal Amount { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }
    }
}