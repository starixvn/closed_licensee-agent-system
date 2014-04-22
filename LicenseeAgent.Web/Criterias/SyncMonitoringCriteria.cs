// ----------------------------------------------------------------------
// <copyright file="SyncMonitoringCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/04/22">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Web.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class SyncMonitoringCriteria : CriteriaBase
    {
        public int ActionID { get; set; }

        public string TaiwanAPIUrl { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SyncMonitoring";
        }

        public override bool IsValid()
        {
            return ActionID >= 0 && !string.IsNullOrEmpty(TaiwanAPIUrl);
        }
    }
}
