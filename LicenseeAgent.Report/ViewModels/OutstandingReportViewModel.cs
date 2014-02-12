// ----------------------------------------------------------------------
// <copyright file="OutstandingReportViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/02/12">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Entities;

    /// <summary>
    /// Bet List View Model.
    /// </summary>
    public class OutstandingReportViewModel
    {
        public int AgentId { get; set; }

        public int AgentLevel { get; set; }

        public int NumberOfLevel { get; set; }
        
        public string BreadCrumb { get; set; }

        public IEnumerable<AgentOutstanding> AgentOutstandings { get; set; }
    }
}
