// --------------------------------------------------------------------------
// <copyright file="HomeViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.ViewModels
{
    using LicenseeAgent.Core.Enumerations;

    public class HomeViewModel
    {
        public TopMenu TopMenu { get; set; }

        public bool IsAgentChangePassword { get; set; }
    }
}