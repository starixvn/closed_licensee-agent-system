// --------------------------------------------------------------------------
// <copyright file="HomeViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.ViewModels
{
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// Modal view modal.
    /// </summary>
    public class HomeViewModel
    {
        public TopMenu TopMenu { get; set; }

        public bool IsAgentChangePassword { get; set; }
    }
}