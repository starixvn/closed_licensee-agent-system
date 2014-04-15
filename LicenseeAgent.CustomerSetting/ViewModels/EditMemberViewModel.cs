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
    /// EditMemberViewModel class.
    /// </summary>
    public class EditMemberViewModel
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int MemberId { get; set; }

        /// <summary>
        /// Gets or sets the agent identifier. Admin use this agent on behalf.
        /// </summary>
        /// <value>
        /// The agent identifier.
        /// </value>
        public int? AgentId { get; set; }

        /// <summary>
        /// Gets or sets the agent level. The level is associated with agent ID.
        /// </summary>
        /// <value>
        /// The agent level.
        /// </value>
        public short? AgentLevel { get; set; }
    }
}