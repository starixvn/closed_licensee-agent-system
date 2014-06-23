// ----------------------------------------------------------------------
// <copyright file="SyncMonitoringGetAPIUrlCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/05/15">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Web.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SyncMonitoringGetAPIUrlCriteria class.
    /// </summary>
    public class SyncMonitoringGetAPIUrlCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Las_SyncMonitoring_GetAPIUrls";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}