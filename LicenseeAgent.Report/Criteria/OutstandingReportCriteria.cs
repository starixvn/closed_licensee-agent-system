// ----------------------------------------------------------------------
// <copyright file="OutstandingReportCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Outstanding Report Criteria.
    /// </summary>
    public class OutstandingReportCriteria : CriteriaBase
    {
        public int AgentId { get; set; }
        
        public bool IsLastLevel { get; set; }

        public override string GetSettingKey()
        {
            return "Las_Rpt_Outstanding";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
