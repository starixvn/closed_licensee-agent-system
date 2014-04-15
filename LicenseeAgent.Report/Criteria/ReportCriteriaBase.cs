// ----------------------------------------------------------------------
// <copyright file="ReportCriteriaBase.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// ReportCriteriaBase class.
    /// </summary>
    public abstract class ReportCriteriaBase : CriteriaBase
    {
        public bool IsShowAll { get; set; }
    }
}
