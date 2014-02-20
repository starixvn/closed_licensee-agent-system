// ----------------------------------------------------------------------
// <copyright file="StatementReportViewModel.cs" company="Nexcel Solutions Vietnam">
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Entities;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;

    /// <summary>
    /// Statement Report View Model.
    /// </summary>
    public class StatementReportViewModel
    {
        public int AgentId { get; set; }

        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }

        public string Username { get; set; }
    }
}
