// --------------------------------------------------------------------------
// <copyright file="EditAgentViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// EditAgentViewModel class.
    /// </summary>
    public class EditAgentViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int AgentId { get; set; }

        [Required]
        [Range(1, short.MaxValue)]
        public int AgentLevel { get; set; }

        public bool? IsEditProduct { get; set; }
    }
}