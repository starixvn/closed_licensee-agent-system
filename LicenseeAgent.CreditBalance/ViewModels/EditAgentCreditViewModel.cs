// --------------------------------------------------------------------------
// <copyright file="EditAgentCreditViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditAgentCreditViewModel
    {
        [Required]
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public decimal? MaxCreditOutput { get; set; }

        public decimal MinCreditOutput { get; set; }

        public bool IsAccountInfo { get; set; }        

        [Required]
        [Display(Name = "Amount", ResourceType = typeof(Las))]
        public decimal GivenCredit { get; set; }
    }
}