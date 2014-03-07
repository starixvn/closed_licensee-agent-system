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

    /// <summary>
    /// Bet List View Model.
    /// </summary>
    public class ResultsReportViewModel
    {
        public ResultsReportViewModel()
        {
            Sports = new List<SelectListItem>();
            Leagues = new List<SelectListItem>();
        }

        public DateTime EventDate { get; set; }

        public int SportType { get; set; }

        public IList<SelectListItem> Sports { get; set; }

        public IList<SelectListItem> Leagues { get; set; }
    }
}
