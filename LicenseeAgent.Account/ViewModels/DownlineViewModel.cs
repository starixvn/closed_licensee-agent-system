// ----------------------------------------------------------------------
// <copyright file="DownlineViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DownlineViewModel
    {
        public int AgentId { get; set; }

        public int? PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
