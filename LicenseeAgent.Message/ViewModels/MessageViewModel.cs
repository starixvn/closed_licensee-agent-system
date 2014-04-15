// -----------------------------------------------------------------------
// <copyright file="MessageViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
            FromDate = DateTime.Now.AddDays(-1);
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