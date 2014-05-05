// ----------------------------------------------------------------------
// <copyright file="SyncMonitoringUpdateRunCountCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/05/05">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Web.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SyncMonitoringUpdateRunCountCriteria class.
    /// </summary>
    public class SyncMonitoringUpdateRunCountCriteria : CriteriaBase
    {
        public int KeyID { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SyncMonitoring_UpdateRunCount";
        }

        public override bool IsValid()
        {
            return KeyID > 0;
        }
    }
}
