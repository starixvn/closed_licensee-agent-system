// ----------------------------------------------------------------------
// <copyright file="OutstandingReportCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/02/12">Created</change>
//   <change who="" date=""></change>
// </history>

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
