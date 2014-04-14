// ----------------------------------------------------------------------
// <copyright file="DownlineViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.ViewModels
{
    public class DownlineViewModel
    {
        public int AgentId { get; set; }

        public int? PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}