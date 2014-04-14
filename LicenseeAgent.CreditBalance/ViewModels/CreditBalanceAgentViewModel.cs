// --------------------------------------------------------------------------
// <copyright file="CreditBalanceAgentViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    public class CreditBalanceAgentViewModel
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