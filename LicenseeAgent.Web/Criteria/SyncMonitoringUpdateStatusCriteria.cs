// ----------------------------------------------------------------------
// <copyright file="SyncMonitoringUpdateStatusCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/04/22">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Web.Criteria
{
    using Fanex.Data.Repository;
    using LicenseeAgent.Taiwan.Enumerations;

    /// <summary>
    /// SyncMonitoringUpdateStatusCriteria class.
    /// </summary>
    public class SyncMonitoringUpdateStatusCriteria : CriteriaBase
    {
        public int UserID { get; set; }

        public int KeyID { get; set; }

        public LogType NewStatusID { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SyncMonitoring_UpdateLogStatus";
        }

        public override bool IsValid()
        {
            return KeyID > 0;
        }
    }
}