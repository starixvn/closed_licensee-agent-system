// --------------------------------------------------------------------------
// <copyright file="LicenseeEditAgentCreditViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseeEditAgentCreditViewModel
    {
        [Required]
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public decimal? MaxCreditOutput { get; set; }

        public decimal MinCreditOutput { get; set; }

        public decimal GivenCredit { get; set; }
    }
}