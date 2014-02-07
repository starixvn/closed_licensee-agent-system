// --------------------------------------------------------------------------
// <copyright file="EditMemberViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditMemberViewModel
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int MemberId { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int AgentId { get; set; }

        [Required]
        [Range(1, short.MaxValue)]
        public short AgentLevel { get; set; }
    }
}