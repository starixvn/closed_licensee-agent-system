// --------------------------------------------------------------------------
// <copyright file="EditAgentViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditAgentViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int AgentId { get; set; }

        [Required]
        [Range(1, short.MaxValue)]
        public int AgentLevel { get; set; }
    }
}