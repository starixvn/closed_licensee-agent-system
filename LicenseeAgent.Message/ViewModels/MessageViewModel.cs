// -----------------------------------------------------------------------
// <copyright file="MessageViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2014.02.24">Created and Implement services for the message.</change>
// </history>

namespace LicenseeAgent.Message.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// Message view model.
    /// </summary>
    public class MessageViewModel
    {
        public MessageViewModel()
        {
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
        }

        [Required]
        [Display(Name = "From", ResourceType = typeof(Las))]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To", ResourceType = typeof(Las))]
        public DateTime ToDate { get; set; }
    }
}