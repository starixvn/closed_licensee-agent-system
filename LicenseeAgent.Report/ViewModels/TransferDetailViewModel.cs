// ----------------------------------------------------------------------
// <copyright file="TransferDetailViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/17">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.ViewModels
{
    using System;

    /// <summary>
    /// Statement Report View Model.
    /// </summary>
    public class TransferDetailViewModel
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsMember { get; set; }
    }
}
