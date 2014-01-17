// ----------------------------------------------------------------------
// <copyright file="ResultsReportViewModel.cs" company="Nexcel Solutions Vietnam">
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
    using System.Web.Mvc;
    using Entities;

    /// <summary>
    /// Bet List View Model.
    /// </summary>
    public class ResultsReportViewModel
    {
        public DateTime EventDate { get; set; }

        public int SportType { get; set; }

        public List<SelectListItem> Sports { get; set; }

        public List<SelectListItem> Leagues { get; set; }
    }
}
