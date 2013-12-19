// ----------------------------------------------------------------------
// <copyright file="BetListCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/19">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Criterias
{
    using System;
    using System.ComponentModel;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListCriteria : CriteriaBase
    {
        private readonly DateTime _oldDate = new DateTime(2008, 12, 31);

        public BetListCriteria()
        {
            var filterDate = DateTime.Today;
            var lastMonth = filterDate.AddMonths(-1);
            Year = filterDate.Year;
            Month = filterDate.Month;
            FromDay = 1;
            var endOfLastMonth = new DateTime(Year, Month, 1).AddMonths(1).AddDays(-1);
            ToDay = filterDate == lastMonth ? endOfLastMonth.Day : filterDate.Day;
            ReportType = 1;
            DrawFilter = 1;
            BaseCurrency = "EUR";
            ShowSB = false;
            ShowRB = false;
            ShowNG = false;
            ShowBA = false;
            ShowLC = false;
            ShowVS = false;
            ShowCS = false;
            ShowBG = false;
        }

        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public int LicenseeMiniumFee { get; set; }

        public string BaseCurrency { get; set; }

        public string CurrencyName { get; set; }

        [DefaultValue(1)]
        public int ReportType { get; set; }

        [DefaultValue(1)]
        public int DrawFilter { get; set; }

        public bool IsExcludeTestData { get; set; }

        public int DateOption { get; set; }

        public bool ShowContent { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int FromDay { get; set; }

        public int ToDay { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int UserId { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string ReportName { get; set; }

        public int DrillDownCurrency { get; set; }

        public DateTime DrillDownDate { get; set; }

        // This property is for drilldown only.
        public DateTime DrillDownFromDate { get; set; }

        // This property is for drilldown only
        public DateTime DrillDownToDate { get; set; }

        public bool ShowSB { get; set; }

        public bool ShowRB { get; set; }

        public bool ShowNG { get; set; }

        public bool ShowBA { get; set; }

        public bool ShowLC { get; set; }

        public bool ShowVS { get; set; }

        public bool ShowCS { get; set; }

        public bool ShowBG { get; set; }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Aff_BetList";
        }
    }
}
