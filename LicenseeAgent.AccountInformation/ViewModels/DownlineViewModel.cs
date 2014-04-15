// ----------------------------------------------------------------------
// <copyright file="DownLineViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.ViewModels
{
    /// <summary>
    /// DownLineViewModel class.
    /// </summary>
    public class DownLineViewModel
    {
        public int AgentId { get; set; }

        public int? PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}