// ----------------------------------------------------------------------
// <copyright file="SyncMonitoring.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/04/22">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Web.Entities
{
    /// <summary>
    /// Monitoring Data's synchronization.
    /// </summary>
    public class SyncMonitoring
    {
        public int CustID { get; set; }

        public string ActionName { get; set; }

        public int ActionID { get; set; }

        public string Command { get; set; }

        public string Remark { get; set; }

        public string Url { get; set; }
    }
}